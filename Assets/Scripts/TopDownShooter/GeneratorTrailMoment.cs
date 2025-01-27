using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class GeneratorTrailMoment : MonoBehaviour
{

    [SerializeField] private float _animationTime;
    [SerializeField] private CinemachineCamera _virtualCamera;
    [SerializeField] private SplineAnimate _splineAnimate;
    [SerializeField] private MeshRenderer _meshCableToRail;

    [Space(10), Header("OnComplet")] 
    [SerializeField] private MeshRenderer _meshCableToDoor;
    [SerializeField] private TopDownConsole _doorConsole;
    [SerializeField] private Animator _animator;
    public UnityEvent _onComplet;

    private float _timer;
    private bool _isInAnimation;

    private void Start() {
        _virtualCamera.Priority = 0;
        _splineAnimate.Duration = _animationTime;
        _meshCableToRail.material.SetFloat("_EnergieOn", 0);
        _meshCableToDoor.material.SetFloat("_EnergieOn", 0);
        if (_animator!=null) _animator.SetBool("IsClose", false);
    }

    public void StartMoment() {
        _splineAnimate.Play();
        _virtualCamera.Priority = 15;
        _meshCableToRail.material.SetFloat("_EnergieOn", 1);
        _isInAnimation = true;
    }
    
    private void Complete() {
        _doorConsole.ChangeStat(TopDownConsole.Stat.OnLine);
        _meshCableToDoor.material.SetFloat("_EnergieOn", 1);
        _virtualCamera.Priority = 0;
        _isInAnimation = false;
        _onComplet?.Invoke();
        if (_animator!=null) _animator.SetBool("IsClose", true);
    }

    private void Update() {
        if (!_isInAnimation) return;
        _timer += Time.deltaTime;
        if (_timer >= _animationTime) {
            Complete();
        }
    }
}