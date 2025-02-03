using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


[RequireComponent(typeof(CharacterController))]
public class TopDownPuzzle : MonoBehaviour
{

    [SerializeField] private bool _isControlled; 
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private GameObject _controlleDisplay;
    [SerializeField] private TopDownPuzzle _topDownPuzzle;
    [SerializeField] private Animator _animator;
    private CharacterController _characterController;

    private bool hadSwitched; 
    void Start() {
        _characterController = GetComponent<CharacterController>();
        SetControlled(_isControlled);
    }

    // Update is called once per frame
    void Update() {
        if (!_isControlled) return;
        ManageMovement();

        if (Input.GetKeyDown(KeyCode.Tab)&&!hadSwitched) {
            if (_topDownPuzzle != null) {
                _topDownPuzzle.SetControlled(true);
                SetControlled(false);
            }

            Debug.Log("Switch");
            if (_animator) _animator.SetTrigger("DoTransfer");
        }
        if (Input.GetButtonDown("Fire1"))if (_animator) _animator.SetTrigger("DoEmote");
        
    }

    private void LateUpdate() {
        hadSwitched = false;
    }


    protected virtual void ManageMovement() {
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _characterController.Move(moveVector.normalized * _moveSpeed * Time.deltaTime);
        if (_animator == null) return;
        bool isWalking = moveVector.magnitude > 0.5f;
        if( isWalking) _animator.transform.forward = moveVector;
        _animator.SetBool("IsWalking", isWalking);
    }

    public void SetControlled(bool controlled) {
        _isControlled = controlled;
        _controlleDisplay.SetActive(controlled);
        hadSwitched = controlled;
    }
}
