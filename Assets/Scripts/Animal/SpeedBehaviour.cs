using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(AnimalStatus))]
//[RequireComponent(typeof(AnimalBehaviour))]

public class SpeedBehaviour : MonoBehaviour
{
    float baseSpeed;
    float runSpeed;

    NavMeshAgent agent;
    AnimalStatus status;
    AnimalBehaviour animal;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<AnimalStatus>();
        animal = GetComponent<AnimalBehaviour>();
        baseSpeed = agent.speed * status.stats.walkSpeedMultiplier;
        runSpeed = agent.speed * status.stats.runSpeedMultiplier;
    }

    void Update() //handle speed 
    {
        switch (animal.GetBehaviour())
        {
            case Behaviour.Fighting:
            case Behaviour.Fleeing:
                agent.speed = runSpeed;
                break;
            default:
                agent.speed = baseSpeed;
                break;
        }
    }
}
