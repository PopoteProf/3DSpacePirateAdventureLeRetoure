using UnityEngine;

public class TopDownRootMotionStupidMonster : TopDownStupidMonsters
{

    [SerializeField] private float _acceleration = 5;
    [SerializeField] private Collider _collider;
    private float _currentSpeed;
    
    protected override void ManagedMovement() {
        
        if (_isDead) return;
        bool isInChaseRange = Vector3.Distance(_player.transform.position, transform.position) <= _aggroDistance;
        bool isInAttackRange = Vector3.Distance(_player.transform.position, transform.position) <= _attackDistance;
        
        if( isInChaseRange&&!isInAttackRange) {
            _currentSpeed += _acceleration * Time.deltaTime;
        }
        else {
            _currentSpeed -= _acceleration * Time.deltaTime;
        }

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _moveSpeed);

        if (_currentSpeed > 0.2) {
            Vector3 forward = _player.transform.position - transform.position;
            forward.y = 0;
            transform.forward = forward;
        }
       
        if (isInAttackRange && _canAttack) {
            if (_animator)_animator.SetTrigger("Attack");
            _attackTimer = 0;
        }

        if (!_canAttack) {
            _attackTimer += Time.deltaTime;
        }
        if (_animator)_animator.SetFloat("Speed", _currentSpeed);
    }
    public override void SetDeath() {
        _collider.enabled = false;
        base.SetDeath();
    }
}