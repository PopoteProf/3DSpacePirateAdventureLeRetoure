using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAggentFollow : MonoBehaviour
{
    public NavMeshAgent MonNavMeshAgent;
    public Transform MaDestination;

  

    private void Update(){
        MonNavMeshAgent.SetDestination(MaDestination.position);
    }
}
