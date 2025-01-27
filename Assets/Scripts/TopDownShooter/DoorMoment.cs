using System;
using Unity.Cinemachine;
using UnityEngine;

public class DoorMoment : MonoBehaviour{
    [SerializeField] private float _momentTime = 34;
    [SerializeField] private Vector3 _doorEndPos;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private Transform _door;
    [SerializeField] private CinemachineImpulseSource _cISRumble;
    [SerializeField] private CinemachineImpulseSource _cIS_FinalBumb;
    [SerializeField] private CinemachineCamera _vCam;
    [SerializeField] private AudioElement _music;
    [SerializeField] private ParticleSystem _doorSparks;
    [SerializeField] private GameObject[] _gameObjectToActivate;


    private bool _isActivated;
    private Vector3 _doorStartPos;
    private float _time;

    private void Start() {
        _doorStartPos = _door.position;
        ActivateGameObject(false);
        if(_vCam)_vCam.Priority = 0;
        
    }

    public void Activate() {
        _isActivated = true;
        if(_cISRumble)_cISRumble.GenerateImpulse();
        if(_vCam)_vCam.Priority = 15;
        AudioManager.Instance.PlayMusic(_music.GetSound(), 1, _music.Volume);
        _doorSparks.Play();
        ActivateGameObject(true);
        
    }

    private void ActivateGameObject(bool value) {
        foreach (var go in _gameObjectToActivate) {
            if (go == null) continue;
            go.SetActive(value);
        }
    }

    private void Update()
    {
        if (!_isActivated) return;
        _time += Time.deltaTime;
        _door.position = Vector3.Lerp(_doorStartPos, _doorEndPos, _animationCurve.Evaluate(_time / _momentTime));
        if (_time >= _momentTime) {
            _door.position = _doorEndPos;
            if(_cIS_FinalBumb)_cIS_FinalBumb.GenerateImpulse();
            if(_vCam)_vCam.Priority = 0;
            _isActivated = false;
            _doorSparks.Stop();

        }
    }
}