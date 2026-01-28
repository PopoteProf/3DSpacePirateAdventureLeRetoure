using System;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public bool PlayerHaveController = true;
    public CharacterController Controller;
    public float MoveSpeed;
    public float SpriteSpeed;

    public float Gravity = -9.81f;
    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    public float JumpHeight = 1f;
    [SerializeField] private Animator _animator;

    [Space(5), Header("AutoClimb")] 
    [SerializeField] private bool _usAutoClimb = true;
    [SerializeField] private Transform _climbRayCaster;
    [SerializeField] private float _climRayDistance= 0.5f;
    [SerializeField] private float _climbDistance= 0.3f;
    [SerializeField] private float _climbTime= 1f;
    [SerializeField] private float _climbTargetOffSet= 1f;
    [SerializeField] private AnimationCurve _climbYCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space(5), Header("AutoHighClimb")]
    [SerializeField] private float _highClimbTime= 1f;
    [SerializeField] private float _highClimbThreshold = 1;
    [SerializeField] private AnimationCurve _highClimbYCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private AnimationCurve _highClimbXZCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private bool _usHandPosition;
    [SerializeField] private Transform _transformRightHand;
    [SerializeField] private AnimationCurve _HandContactAniamtionCurve; 
    [SerializeField] private Transform _transformRightFoot;
    [SerializeField] private AnimationCurve _FootContactAniamtionCurve;  
    [Space(5),Header("Auto Crouch Settings")]
    [SerializeField] private bool _usAutoCrouch = true;
    [SerializeField] private float _mintriggerCrouchHeitgh= 1f;
    [SerializeField] private float _maxtriggerCrouchHeitgh= 2f;
    [SerializeField] private float _defaultHeight= 1.7f;
    [SerializeField] private Vector3 _defaultcCenter= new Vector3(0,-0.18f,0);
    [SerializeField] private float _crouchHeight= 1.2f;
    [SerializeField] private Vector3 _crouchCenter= new Vector3(0,-0.12f,0);
    [SerializeField] private bool _displayAutoCrouchDebug;
    
    [Space(5), Header("DialogueSettings")]
    [SerializeField] private float _dialogueSetupMoveSpeed = 2;
    [SerializeField] private AnimationCurve _dialogueSetupAnimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    public Transform DialoguePlayerCamTransform;
    
    private bool _doAutoClimb;
    private bool _doAutohighClimb;
    private Vector3 _autoClimbStart;
    private Vector3 _autoClimbTarget;
    private float _autoclimbTimer;
    private bool _doDialogueSetUp;
    private float _dialogueSetupTime;
    private Vector3 _lookAtPos;
    private Vector3 _climCorner;

    public event EventHandler OnJump;
    public event EventHandler OnCimb;
    public event EventHandler OnLanding;
    


    // Est vrai si le player touche le sol
    protected bool _isGRounded;

    // La vitesse vertical du joueur
    protected Vector3 _velocity;

    // La direction de movement du joueur
    protected Vector3 _moveVector;

    protected virtual void Update()
    {
        
        if(_doDialogueSetUp)ManageADialogueSetup();
        if (!PlayerHaveController) return;
        if (_doAutoClimb) {
            ManageAutoClimb();
            return;
        }

        if (_doAutohighClimb) {
            ManageAutoHighClimb();
            return;
        }
        ManagerIsGrounded();
        ManageMovement();
        if( _usAutoClimb) ManageAutoClimbDetection();
        if (_usAutoCrouch) ManageAutoCrouch();
        
        
    }

    private void ManagerIsGrounded()
    {
        bool isground =Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if( !_isGRounded&& isground) OnLanding?.Invoke(this , EventArgs.Empty);
        _isGRounded = isground;
    }
    
    protected virtual void ManageMovement() {
        _moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
       

        if (_isGRounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump")&&_isGRounded ) {
            _velocity.y = JumpHeight;
            Debug.Log("Jump" );
            if(_animator)_animator.SetTrigger("Jump");
            OnJump?.Invoke(this , EventArgs.Empty);
        }

        _velocity.y += Gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift)) {
            Controller.Move(SpriteSpeed * Time.deltaTime * _moveVector);
        }
        else {
            Controller.Move(MoveSpeed * Time.deltaTime * _moveVector);
        }

        if (_animator) _animator.SetFloat("Speed", Controller.velocity.magnitude);
        

        if (Controller.velocity.magnitude > 0.2) {
           Vector3 forward = Controller.velocity;
           forward.y = 0;
           transform.forward = forward;
        }

        Controller.Move(_velocity * Time.deltaTime);
       if (_animator)
       {
           _animator.SetBool("IsGrounded", _isGRounded);
           _animator.SetFloat("YSpeed", Controller.velocity.y);
       }
       
    }

    private void ManageAutoClimbDetection() {
        Debug.DrawLine(_climbRayCaster.position, _climbRayCaster.position + _climbRayCaster.forward * _climRayDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(_climbRayCaster.position, _climbRayCaster.forward, out hit, _climRayDistance, GroundMask)) {
            RaycastHit hit2;
            Vector3 newRayCater = hit.point + new Vector3(0, 2, 0) + _climbRayCaster.forward*_climbDistance;
            if (Physics.Raycast(newRayCater, -transform.up, out hit2, 5,
                GroundMask)) {
                Debug.DrawLine(newRayCater, hit2.point, Color.yellow);
                if (!Physics.CheckCapsule(hit2.point + new Vector3(0, 0.6f, 0), hit2.point + new Vector3(0, 1.4f, 0), 0.2f)) {
                    Debug.DrawLine(hit2.point +_climbRayCaster.forward*0.2f,hit2.point +_climbRayCaster.forward*-0.2f, Color.green);
                    
                    
                    if (Physics.Raycast(
                        _climbRayCaster.position + new Vector3(0, 1, 0),
                        hit2.point + new Vector3(0, 1, 0) - _climbRayCaster.position + new Vector3(0, 1, 0),
                        (hit2.point + new Vector3(0, 1, 0) - _climbRayCaster.position + new Vector3(0, 1, 0))
                        .magnitude))
                        return;

                    if (hit2.point.y - transform.position.y > _highClimbThreshold) {
                        _doAutohighClimb = true;
                        _climCorner = hit.point;
                        _climCorner.y = hit2.point.y;
                        if(_animator)_animator.SetBool("DoHighClimb", true);
                        OnCimb?.Invoke(this , EventArgs.Empty);
                    }
                    else
                    {
                        if(_animator)_animator.SetBool("DoClimbStep", true);
                        OnJump?.Invoke(this , EventArgs.Empty);
                        _doAutoClimb = true;
                    }

                    _velocity.y = 0;
                    _autoClimbStart = transform.position;
                    _autoClimbTarget = hit2.point + new Vector3(0, _climbTargetOffSet, 0);
                    _autoclimbTimer = 0;
                    
                    
                }
            }
            
        }
    }

    private void ManageAutoClimb() {
        _autoclimbTimer += Time.deltaTime;
        Vector3 pos =  Vector3.Lerp(_autoClimbStart, _autoClimbTarget, _autoclimbTimer / _climbTime);
        pos.y = Mathf.Lerp(_autoClimbStart.y, _autoClimbTarget.y, _climbYCurve.Evaluate(_autoclimbTimer / _climbTime));
        transform.position = pos;
        
        if (_autoclimbTimer >= _climbTime) {
            _doAutoClimb = false;
            if(_animator)_animator.SetBool("DoClimbStep", false);
        }
    }
    private void ManageAutoHighClimb() {
        _autoclimbTimer += Time.deltaTime;
        float t = _autoclimbTimer / _highClimbTime;
        Vector3 pos =  Vector3.Lerp(_autoClimbStart, _autoClimbTarget, _highClimbXZCurve.Evaluate(t));
        pos.y = Mathf.Lerp(_autoClimbStart.y, _autoClimbTarget.y, _highClimbYCurve.Evaluate(t));

        if (_usHandPosition) {
            float handRoot = _animator.GetFloat("HandRoot");
            Vector3 handOffset = transform.position - _transformRightHand.position;
            Vector3 footOffset = transform.position - _transformRightFoot.position;
            Vector3 offset = Vector3.Lerp(handOffset, footOffset,
                _HandContactAniamtionCurve.Evaluate(t) / _FootContactAniamtionCurve.Evaluate(t));
            Debug.DrawLine(transform.position, _climCorner, Color.green);
           pos=Vector3.Lerp(pos, _climCorner + handOffset, handRoot);
        }

        transform.position = pos;
        
        if (_autoclimbTimer >= _highClimbTime) {
            _doAutohighClimb = false;
            if(_animator)_animator.SetBool("DoHighClimb", false);
        }
    }
    
    private void ManageADialogueSetup() {
        _autoclimbTimer += Time.deltaTime;
        transform.position =  Vector3.Lerp(_autoClimbStart, _autoClimbTarget, _dialogueSetupAnimationCurve.Evaluate(_autoclimbTimer / _dialogueSetupTime));
        Vector3 forward = _lookAtPos - transform.position;
        forward.y = 0;
        transform.forward = forward;
        if(_animator)_animator.SetFloat("Speed", _dialogueSetupMoveSpeed);
        if (_autoclimbTimer >= _dialogueSetupTime) {
            if(_animator)_animator.SetFloat("Speed",0);
            _doDialogueSetUp = false;
        }
    }

    private void ManageAutoCrouch() {
        float croutAmout;
        if (Physics.Raycast(new Ray(transform.position, transform.up), out RaycastHit hit, _maxtriggerCrouchHeitgh,
                GroundMask)) {
            croutAmout = 1-Mathf.Clamp01(hit.distance-_mintriggerCrouchHeitgh)/(_maxtriggerCrouchHeitgh-_mintriggerCrouchHeitgh);
        }
        else {
            croutAmout = 0;
        }

        Controller.center = Vector3.Lerp(_defaultcCenter, _crouchCenter, croutAmout);
        Controller.height = Mathf.Lerp(_defaultHeight, _crouchHeight, croutAmout);

        if(_animator)_animator.SetFloat("CrouchAmout", croutAmout);
        
        if (_displayAutoCrouchDebug) {
            Debug.DrawLine(transform.position,transform.position+new Vector3(0,Mathf.Lerp(_maxtriggerCrouchHeitgh, _mintriggerCrouchHeitgh, croutAmout)) , Color.red);
        }
    }

    public void SetUpForDialogue(Vector3 posTarget, float time, Vector3 lookAtPos) {
        PlayerHaveController = false;
        _doDialogueSetUp = true;
        _autoClimbTarget = posTarget;
        _autoClimbStart = transform.position;
        _dialogueSetupTime = time;
        _lookAtPos = lookAtPos;
        _autoclimbTimer = 0;
    }

    public void SetPlayerIsTalking(bool value) {
        if(_animator)_animator.SetBool("IsTalking" ,value);
    }

    public void EndDialogue()
    {
        PlayerHaveController = true;
    }

    public void Respawn()
    {
        PlayerHaveController = false;
        Controller.transform.position = PlatformerCheckPoint.RESPAWNPOINT.position;
        Invoke("EndDialogue",1);
        //transform.forward = PlatformerCheckPoint.RESPAWNPOINT.forward;
    }
}