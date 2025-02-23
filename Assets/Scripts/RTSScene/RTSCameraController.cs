using System;
using UnityEditor.Searcher;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private bool _displayDebugGizmo;

    [SerializeField] private int _horizontalBorderd = 10;
    [SerializeField] private int _VerticalBorderd = 10;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _cameraSpeed = 5;

    [Space(10), Header("CameraBoundry")]
    [SerializeField] private Vector3 _worldOffSet;
    [SerializeField] private Vector2Int _cameraBoundrySize = new Vector2Int(10, 10);
    
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 moveVector = Vector3.zero;

        if (Input.mousePosition.x < _horizontalBorderd) {
            moveVector.x = -(1-Input.mousePosition.x / _horizontalBorderd);
        }

        if (Input.mousePosition.x > (Screen.width - _horizontalBorderd)) {
            moveVector.x =( Input.mousePosition.x - (Screen.width - _horizontalBorderd)) / _horizontalBorderd;
        }
        
        if (Input.mousePosition.y < _VerticalBorderd) {
            moveVector.z = -(1-Input.mousePosition.y/ _VerticalBorderd);
        }

        moveVector.x += Input.GetAxis("Horizontal");
        moveVector.z += Input.GetAxis("Vertical");
        if (Input.mousePosition.y > (Screen.height - _VerticalBorderd)) {
            moveVector.z = (Input.mousePosition.y - (Screen.height - _VerticalBorderd)) / _VerticalBorderd;
        }
        moveVector = Vector3.ClampMagnitude(moveVector , 1);

        Vector3 newPos = _cameraTarget.position + _cameraSpeed * Time.deltaTime * moveVector;

        newPos.x = Mathf.Clamp(newPos.x, _worldOffSet.x, _worldOffSet.x + _cameraBoundrySize.x);
        newPos.z = Mathf.Clamp(newPos.z, _worldOffSet.z, _worldOffSet.z + _cameraBoundrySize.y);
        
        _cameraTarget.position = newPos;
    }

    private void OnDrawGizmos()
    {
        if (!_displayDebugGizmo) return;
        if (_mainCamera == null) return;
        
        Vector3 testPos = Vector3.zero;
        testPos.x = Screen.width-_horizontalBorderd;
        testPos.y = Screen.height -_VerticalBorderd;
        testPos.z = _mainCamera.nearClipPlane + 1;
        Vector3 pos1 = _mainCamera.ScreenToWorldPoint(testPos);
        testPos.x = Screen.width-_horizontalBorderd;
        testPos.y = _VerticalBorderd;
        Vector3 pos2 = _mainCamera.ScreenToWorldPoint(testPos);
        testPos.x =_horizontalBorderd;
        testPos.y = _VerticalBorderd;
        Vector3 pos3 = _mainCamera.ScreenToWorldPoint(testPos);
        testPos.x = _horizontalBorderd;
        testPos.y = Screen.height -_VerticalBorderd;
        Vector3 pos4 = _mainCamera.ScreenToWorldPoint(testPos);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine( pos1,pos2);
        Gizmos.DrawLine( pos2,pos3);
        Gizmos.DrawLine( pos3,pos4);
        Gizmos.DrawLine( pos4,pos1);

        pos1 =pos2 = pos3 =pos4= _worldOffSet;
        pos2.x += _cameraBoundrySize.x;
        pos3.x += _cameraBoundrySize.x;
        pos3.z += _cameraBoundrySize.y;
        pos4.z += _cameraBoundrySize.y;
        
        Gizmos.DrawLine( pos1,pos2);
        Gizmos.DrawLine( pos2,pos3);
        Gizmos.DrawLine( pos3,pos4);
        Gizmos.DrawLine( pos4,pos1);

    }
}
