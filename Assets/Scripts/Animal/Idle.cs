
using UnityEngine;
public class Idle : BaseBehaviour
{
    public Idle(ref AnimalBehaviour animal) : base(ref animal) { }
   
    [SerializeField] float wanderWait = 5.0f;
    float wanderTimePassed = 0;

    public void Wander()
    {
        wanderTimePassed += Time.deltaTime;

        if (wanderTimePassed > wanderWait)
        {
            wanderTimePassed = 0;

            if (animal.agent.remainingDistance < 0.1f)
            {
                ChooseRandomDestination();
            }
        }

    }

    public void ChooseRandomDestination()
    {
        Vector3 randomPos = Random.insideUnitSphere * animal.stats.searchRadius;
        animal.agent.destination = animal.transform.position + randomPos;
    }
}
