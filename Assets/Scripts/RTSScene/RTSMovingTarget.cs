using UnityEngine;
using UnityEngine.AI;

public class RTSMovingTarget : RTSIldeTarget
{
    [SerializeField] private Vector3[] _moveingpoints;
    [SerializeField] private bool _loopPath;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private bool _displayPath;
    private int _currentTargetPosID;

    private bool _isInReverse;

    protected override void Start() {
        
        base.Start();
        SetNewTargetDestination(0);
    }
    
    private void Update()
    {
        if (Vector3.Distance(_navMeshAgent.pathEndPosition, transform.position)<_navMeshAgent.stoppingDistance) {
            SetNextTarget();
        }
    }

    private void SetNextTarget()
    {
        if (_loopPath) {
            _currentTargetPosID++;
            if (_currentTargetPosID >= _moveingpoints.Length) {
                _currentTargetPosID = 0;
            }
        }
        else {
            if (_isInReverse) {
                _currentTargetPosID--;
                if (_currentTargetPosID < 0) {
                    _currentTargetPosID = 1;
                    _isInReverse = false;
                }
            }
            else
            {
                _currentTargetPosID++;
                if (_currentTargetPosID >= _moveingpoints.Length) {
                    _currentTargetPosID = _moveingpoints.Length-2;
                    _isInReverse = true;
                }
            }
        }
        
        SetNewTargetDestination(_currentTargetPosID);
    }

    private void SetNewTargetDestination(int id) {
        _navMeshAgent.SetDestination(_moveingpoints[_currentTargetPosID]);
    }


    private void OnDrawGizmos() {
        if (!_displayPath) return;
        if (_moveingpoints != null && _moveingpoints.Length > 1) {
            for (int i = 0; i < _moveingpoints.Length; i++) {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(_moveingpoints[i], Vector3.one*0.2f);
                if (i > 0) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(_moveingpoints[i-1], _moveingpoints[i]);
                }
            }

            if (_loopPath) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_moveingpoints[0], _moveingpoints[_moveingpoints.Length-1]);
            }
        }
    }
}