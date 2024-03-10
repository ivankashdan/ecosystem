using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fleeing : SearchBehaviour
{
    [SerializeField] float fleeDistanceMultiplier = 30.0f;
    [SerializeField] float fleeWait = 2.0f;
    float fleeTimePassed = 0;

    public Fleeing(ref AnimalBehaviour animal) : base(ref animal) 
    {
        fleeTimePassed = fleeWait;
    }

    public void Flee()
    {
        fleeTimePassed += Time.deltaTime;

        if (fleeTimePassed > fleeWait)
        {
            fleeTimePassed = 0;

            if (IsTargetStillVisible())
            {
                Transform target = animal.GetTarget();

                animal.agent.destination = (animal.transform.position - target.position).normalized * fleeDistanceMultiplier;

            }
        }
    }

}
