using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fleeing : SearchBehaviour, IBehaviour
{
    [SerializeField] float fleeDistanceMultiplier = 30.0f;
    [SerializeField] float fleeWait = 2.0f;
    float fleeTimePassed = 0;

    private void Start()
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

                agent.destination = (transform.position - target.position).normalized * fleeDistanceMultiplier;

            }
        }
    }

}
