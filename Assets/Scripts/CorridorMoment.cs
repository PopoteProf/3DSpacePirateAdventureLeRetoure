using Unity.Cinemachine;
using UnityEngine;

public class CorridorMoment : MonoBehaviour {

    [SerializeField] private CinemachineCamera _vCam;
    public void EnterCorridor() {
        if(_vCam)_vCam.Priority =15;
    }

    public void ExitCorridor() {
        if(_vCam)_vCam.Priority =0;
    }
}