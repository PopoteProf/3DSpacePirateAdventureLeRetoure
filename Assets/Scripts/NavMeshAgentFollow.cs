using System;
using UnityEngine;
using UnityEngine.AI;


public class NavMeshAgentFollow : MonoBehaviour
{
    public NavMeshAgent MonNavMeshAgent;
    public Transform maDestination;

    private void Update()
    {
        MonNavMeshAgent.SetDestination(maDestination.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(maDestination.position, 0.1f);
    }
}
