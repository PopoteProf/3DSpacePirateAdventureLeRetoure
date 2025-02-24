using NUnit.Framework.Interfaces;
using UnityEngine;

public class ThirdPersonRootMotionController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed =1.5f;
    [SerializeField] private float _runAcceleration = 0.2f;
    [SerializeField] private float _runSpeed =2.5f;
    [SerializeField] private float _cameraSpeed=1f;
    [SerializeField] private Transform _aimTarget;
    
    [SerializeField] private float _gravity =-9.8f;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    [Header("Vertical Aim")] 
    [SerializeField] private float _minAimHeight = 0.75f;
    [SerializeField] private float _maxAimHeight = 2;
    [SerializeField] private float _heightCameraSpeed = 50;
    

   
    private float _aimheight=1;

    private bool _isGrounded;
    private Vector3 _velocity;
    private float _currentMoveSpeed;
    
    // Update is called once per frame
    void Update() {
        
        ManagerIsGrounded();
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xrot = Input.GetAxis("Mouse X");
        
        
        
        
        //if (_isGrounded && _velocity.y < 0) {
        //    _velocity.y =-_gravity * Time.deltaTime;
        //}
        //_velocity.y += _gravity * Time.deltaTime;
        //transform.position += _velocity;
        if( Input.GetKeyDown(KeyCode.Q)&& _animator)_animator.SetTrigger("A");
        if( Input.GetKeyDown(KeyCode.E)&& _animator)_animator.SetTrigger("E");
        if( Input.GetKeyDown(KeyCode.R)&& _animator)_animator.SetTrigger("R");
        
        
        Vector3 moveVec = transform.forward * y + transform.right * x;
        if (moveVec.magnitude > 0.2f) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                if (_currentMoveSpeed<_runSpeed)_currentMoveSpeed += _runAcceleration * Time.deltaTime;
                else _currentMoveSpeed -= _runAcceleration * Time.deltaTime;
            }
            else {
                if (_currentMoveSpeed<_speed)_currentMoveSpeed += _runAcceleration * Time.deltaTime;
                else _currentMoveSpeed -= _runAcceleration * Time.deltaTime;
            }
        }
        else {
            _currentMoveSpeed -= _runAcceleration * Time.deltaTime;
            _currentMoveSpeed = Mathf.Clamp(_currentMoveSpeed, 0, _speed);
        }




        transform.Rotate(Vector3.up, xrot*Time.smoothDeltaTime*_cameraSpeed);
        
        
        Vector3 animVec = transform.InverseTransformDirection(moveVec*_currentMoveSpeed);
        if (_animator)_animator.SetFloat("X", animVec.x);
        if (_animator)_animator.SetFloat("Y", animVec.z);
        if (_animator)_animator.SetFloat("Rot", xrot);

        if (_aimTarget) {
            float yRot = Input.GetAxis("Mouse Y");
            _aimheight = Mathf.Clamp(_aimheight + yRot * Time.smoothDeltaTime * _heightCameraSpeed, _minAimHeight,
                _maxAimHeight);
            _aimTarget.localPosition = new Vector3(_aimTarget.localPosition.x, _aimheight, _aimTarget.localPosition.z);

        }

    }
    
    private void ManagerIsGrounded() {
        bool isground =Physics.CheckSphere(_groundChecker.position, _groundCheckerRadius, _groundMask);
        _isGrounded = isground;
    }
}