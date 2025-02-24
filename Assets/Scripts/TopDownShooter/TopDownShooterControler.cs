using System;
using Unity.Cinemachine;
using UnityEngine;

public class TopDownShooterControler : MonoBehaviour
{

    public event EventHandler OnFire;
    
    [SerializeField] private Animator _animator;
    
    [SerializeReference] private CharacterController _characterController;
    [Space(5), Header("Move")]
    [SerializeField] private float _runSpeed = 6;
    [SerializeField] private float _moveAcceleration = 2;
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private float _gravity =-9.8f;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    

    [Space(5), Header("HP")]
    [SerializeField] private float _hpMax = 10;
    [SerializeField] private float _hpCurrent = 10;

    [Space(5), Header("Aiming")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _aimHeightDisplacment = 0.5f;

    [Space(5), Header("Fire")]
    [SerializeField] private TopDownPlayerProjectile _prefabProjectile;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private CinemachineImpulseSource _fireImpulseSource;
    [SerializeField] private TopDownShooterFireLight _fireLight;


    private float _currentSpeed;
    private bool _isGrounded;
    private Vector3 _velocity;
    private Vector3 _aimVector;
    private TopDownConsole _currentTopDownConsole;

    public void Start()
    {
        GameStaticManager.OnPlayerTakeDamage+= GameStaticManagerOnOnPlayerTakeDamage;
    }

    private void GameStaticManagerOnOnPlayerTakeDamage(object sender, float e) {
        _hpCurrent -= e;
        _hpCurrent = Mathf.Clamp(_hpCurrent, 0, _hpCurrent);
        GameStaticManager.SetPlayerHpChange(_hpCurrent/_hpMax);
    }

    void Update() {
        ManagerIsGrounded();
        ManageMovement();
        ManageAim();
        ManageFire();
        ManagerInteraction();
    }

    private void ManagerInteraction()
    {
        if (_currentTopDownConsole != null && Input.GetKeyDown(KeyCode.E)) {
            _currentTopDownConsole.Intract();
            if(_animator)_animator.SetTrigger("Interact");
        }
    }

    private void ManageFire() {
        if (Input.GetButtonDown("Fire1")) {
            TopDownPlayerProjectile projectile =
                Instantiate(_prefabProjectile, _firePoint.position, Quaternion.identity);
            projectile.transform.forward = _aimVector;
            if(_fireImpulseSource)_fireImpulseSource.GenerateImpulse();
            OnFire?.Invoke(this , EventArgs.Empty);
            _fireLight.Fire();
            if(_animator)_animator.SetTrigger("Fire");
        }
    }

    private void ManageAim() {
        Ray ray =_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            Vector3 aimPos;
            if (hit.transform.CompareTag("Monster")) {
                aimPos = hit.transform.position;
            }
            else {
                aimPos= hit.point + new Vector3(0, _aimHeightDisplacment, 0);
            }
            Debug.DrawLine(hit.point, aimPos, Color.yellow);
            Debug.DrawLine(aimPos, _firePoint.position, Color.red);
            Vector3 forward =_aimVector = aimPos - _firePoint.position; 
            forward.y = 0;
            transform.forward = forward;
        }
    }

    protected virtual void ManageMovement() {


        if (Input.GetKey(KeyCode.LeftShift)) {
            _currentSpeed += _moveAcceleration * Time.deltaTime;
        }
        else {
            _currentSpeed -= _moveAcceleration * Time.deltaTime;
        }

        _currentSpeed = Mathf.Clamp(_currentSpeed, _moveSpeed, _runSpeed);
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _characterController.Move(moveVector.normalized * _currentSpeed * Time.deltaTime);
        if (_isGrounded && _velocity.y < 0) {
            _velocity.y = -2f;
        }
        _velocity.y += _gravity * Time.deltaTime;

        _characterController.Move(_velocity * Time.deltaTime);
       
        float angle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.up);

        //Debug.Log( angle);
        Vector3 displayVec = RotateXZ(moveVector, -angle);
        displayVec = displayVec * _currentSpeed;
        if(_animator)_animator.SetFloat("X", displayVec.x);
        if(_animator)_animator.SetFloat("Z", displayVec.z);

    }
    private void ManagerIsGrounded() {
        bool isground =Physics.CheckSphere(_groundChecker.position, _groundCheckerRadius, _groundMask);
        _isGrounded = isground;
    }
    
    public Vector3 RotateXZ(Vector3 originVec, float angle) {
        angle = angle * Mathf.Deg2Rad;
        float x = (originVec.x*Mathf.Cos(angle) - originVec.z*Mathf.Sin(angle));
        float z = (originVec.x*Mathf.Sin(angle) + originVec.z*Mathf.Cos(angle));
        return new Vector3(x, 0, z);
    }

    public void SetConsole(TopDownConsole console, bool set=true) {
        if (set = false && _currentTopDownConsole != console) return;
        if( _currentTopDownConsole!=null) _currentTopDownConsole.ExitConsole();
        _currentTopDownConsole = console;
    }

    public void TakeDamage() {
        if(_animator)_animator.SetTrigger("Hit");
    }
}