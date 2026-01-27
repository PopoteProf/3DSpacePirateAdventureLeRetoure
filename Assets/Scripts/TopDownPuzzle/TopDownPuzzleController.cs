using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


[RequireComponent(typeof(CharacterController))]
[SelectionBase]
public class TopDownPuzzleController : MonoBehaviour
{

    [SerializeField] private bool _isControlled; 
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private GameObject _controlleDisplay;
    [SerializeField] private TopDownPuzzleController topDownPuzzleController;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _emoteTime = 1.2f;
    private CharacterController _characterController;

    private bool IsEmote
    {
        get => _timer != 0;
    }
    private float _timer;
    private bool hadSwitched; 
    void Start() {
        _characterController = GetComponent<CharacterController>();
        SetControlled(_isControlled);
    }

    // Update is called once per frame
    void Update() {
        if (!_isControlled) return;
        ManageMovement();
        ManageWaitTimer();

        if (Input.GetKeyDown(KeyCode.Tab)&&!hadSwitched) {
            if (topDownPuzzleController != null) {
                topDownPuzzleController.SetControlled(true);
                SetControlled(false);
            }

            Debug.Log("Switch");
            if (_animator) _animator.SetTrigger("DoTransfer");
        }
        if (Input.GetButtonDown("Fire1"))
            if (_animator) {
                _timer+=Time.deltaTime;
                _animator.SetTrigger("DoEmote");
            }
        
    }

    private void LateUpdate() {
        hadSwitched = false;
    }


    protected virtual void ManageMovement()
    {
        if (IsEmote) return;
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _characterController.Move(moveVector.normalized * _moveSpeed * Time.deltaTime);
        if (_animator == null) return;
        bool isWalking = moveVector.magnitude > 0.5f;
        if( isWalking) _animator.transform.forward = moveVector;
        _animator.SetBool("IsWalking", isWalking);
    }

    private void ManageWaitTimer() {
        if(!IsEmote) return;
        _timer += Time.deltaTime;
        if (_timer >= _emoteTime) {
            _timer = 0;
        }
    }

    public void SetControlled(bool controlled) {
        _isControlled = controlled;
        _controlleDisplay.SetActive(controlled);
        hadSwitched = controlled;
    }
}
