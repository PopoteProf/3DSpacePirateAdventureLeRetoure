using UnityEngine;

public class RTSBuildingController : MonoBehaviour
{
    [SerializeField] private float _heightThreshHold = 0.2f;
    [SerializeField] private Vector3 _size = Vector3.one;
    [SerializeField] private bool _displayerDebugBox; 
    private RTSBuildingGhost _ghost;

    public bool IsSpaceFree {
        get => _isSpaceFree;
    }
    
    private bool _isSpaceFree;

    public void SetNewSOBuilding(SORTSBuilding soBuilding) {
        if( _ghost!=null) Destroy(_ghost.gameObject);
        _size = soBuilding.Size;
        _ghost = Instantiate(soBuilding.PrefabsGhost, transform);
        _ghost.transform.position = transform.position - new Vector3(0,_heightThreshHold,0);
    }

    public void SetNewPosition(Vector3 pos) {
        transform.position = pos + new Vector3(0,_heightThreshHold/2,0);
    }
    
    private void Update() {
        if (Physics.CheckBox(transform.position+new Vector3(0,_size.y,0), _size)) {
            _isSpaceFree = false;
        }
        else {
            _isSpaceFree = true;
        }

        CheckHeight( new Vector3(_size.x, 0, _size.z));
        CheckHeight( new Vector3(_size.x, 0, -_size.z));
        CheckHeight( new Vector3(-_size.x, 0, -_size.z));
        CheckHeight( new Vector3(-_size.x, 0, _size.z));

        if (_ghost != null) {
            _ghost.ChangeValidateState(IsSpaceFree);
        }
    }

    private void CheckHeight(Vector3 pos) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position+pos, Vector3.down, out hit)) {
            if (Mathf.Abs(hit.point.y - transform.position.y) > _heightThreshHold) {
                _isSpaceFree = false;
            }
        }
        else _isSpaceFree = false;
    }

    private void OnDrawGizmos() {
        if (!_displayerDebugBox) return;
        if( _isSpaceFree) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        
        Gizmos.DrawCube(transform.position+new Vector3(0,_size.y,0), _size*2);
    }
}