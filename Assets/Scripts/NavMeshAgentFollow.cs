using UnityEngine.AI;
using UnityEngine;

public class NavMeshAgentFollow : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform MaDestination;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(MaDestination.position);
    }
    
    
}