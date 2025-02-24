using UnityEngine;
using UnityEngine.AI;

namespace Loan.Script
{
    public class NavMeshAgentFollow : TopDownStupidMonsters
    {
        private NavMeshAgent _navMeshAgent;
        private Transform _target;
        private CharacterController _characterController;
        void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected override void Start()
        {
            _characterController = GetComponent<CharacterController>();
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                _target = player.transform;
            }
            base.Start();
        }
        
        protected override void ManagedMovement()
        {
            if (_isDead) return;
            if( _target != null )
            {
                _navMeshAgent.SetDestination(_target.position);
            
                if (_navMeshAgent.velocity.magnitude > 0.5f)
                {
                    transform.forward = _navMeshAgent.velocity;
                }
                if (_animator)_animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);

            }
        

            if (Vector3.Distance(_target.transform.position, transform.position) <= _attackDistance && _canAttack) {
                if (_animator)_animator.SetTrigger("Attack");
                _attackTimer = 0;
            }

            if (!_canAttack) {
                _attackTimer += Time.deltaTime;
            }
        }
        
        public override void SetDeath() {
            _characterController.enabled = false;
            _navMeshAgent.enabled = false;
            base.SetDeath();
        }
    }
}

