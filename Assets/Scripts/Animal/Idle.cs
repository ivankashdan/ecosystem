
using UnityEngine;
using UnityEngine.AI;


//[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(AnimalStatus))]
public class Idle : MonoBehaviour
{

    [SerializeField] float wanderWait = 5.0f;
    float wanderTimePassed = 0;

    NavMeshAgent agent;
    AnimalStatus status;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<AnimalStatus>();
    }

    public void Wander()
    {
        wanderTimePassed += Time.deltaTime;

        if (wanderTimePassed > wanderWait)
        {
            wanderTimePassed = 0;

            if (agent.remainingDistance < 0.1f)
            {
                ChooseRandomDestination();
            }
        }

    }

    public void ChooseRandomDestination()
    {
        agent.destination = transform.position + (Random.insideUnitSphere * status.stats.searchRadius);
    }
}
