using Unity.Cinemachine;
using UnityEngine;

public class CorridorMoment : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera _vCam;
    public void EnterCorridor() {
        if(_vCam)_vCam.enabled = true;
    }

    public void ExitCorridor() {
        if(_vCam)_vCam.enabled = false;
    }
}