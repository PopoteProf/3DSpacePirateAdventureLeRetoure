using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Nav : TopDownStupidMonsters
{
    public NavMeshAgent _navMeshAgent;
    public Transform target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void ManagedMovement() 
    {
        if (target != null)
        {
            if (_isDead) return;
            
            _navMeshAgent.SetDestination(target.position);

            if (Vector3.Distance(transform.position, target.position) < _attackDistance)
            {
                _animator.SetTrigger("Hit");
            }
        }
    }
}
