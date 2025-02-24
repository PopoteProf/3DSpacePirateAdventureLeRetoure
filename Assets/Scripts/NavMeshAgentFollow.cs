using UnityEngine;
using UnityEngine.AI;

public class NewMonoBehaviourScript : MonoBehaviour
{

	public NavMeshAgent MonNavMeshAgent;
	public Transform MaDestination;
   
	
    void Update()
    {
	    MonNavMeshAgent.SetDestination(MaDestination.position);
    }

    
    
    
    
    
    
    
    



}
