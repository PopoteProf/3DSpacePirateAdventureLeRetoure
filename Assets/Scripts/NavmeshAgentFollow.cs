using System;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshAgentFollow : MonoBehaviour
{
    public Transform MaDestionation;
    public NavMeshAgent MonNavMeshAgent;

    void Start()
    {

    }

    void Update()
    {
        MonNavMeshAgent.SetDestination(MaDestionation.position);
    }
}
