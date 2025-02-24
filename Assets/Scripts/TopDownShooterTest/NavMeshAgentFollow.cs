using UnityEngine;
using UnityEngine.AI;

namespace TopDownShooterTest
{
    public class NavMeshAgentFollow : TopDownStupidMonsters
    {
        private NavMeshAgent _navMeshAgent;
        private Collider _collider;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<Collider>();
        }

        protected override void ManagedMovement()
        {
            if (_isDead) return;
            
            _navMeshAgent.SetDestination(_player.gameObject.transform.position);

            if (_animator)_animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
            
            if (Vector3.Distance(_player.transform.position, transform.position) <= _attackDistance && _canAttack) {
                if (_animator)_animator.SetTrigger("Attack");
                _attackTimer = 0;
            }

            if (!_canAttack) {
                _attackTimer += Time.deltaTime;
            }
        }
        
        public override void SetDeath() {
            _navMeshAgent.ResetPath();
            _collider.enabled = false;
            base.SetDeath();
        }
    }
}
