using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class SpeedBehaviour : BaseBehaviour
{
    float baseSpeed;
    float runSpeed;

    public SpeedBehaviour(ref AnimalBehaviour animal) : base(ref animal)
    {
        baseSpeed = animal.agent.speed * animal.stats.walkSpeedMultiplier;
        runSpeed = animal.agent.speed * animal.stats.runSpeedMultiplier;
    }


    void Update() //handle speed 
    {
        switch (animal.GetBehaviour())
        {
            case Behaviour.Fighting:
            case Behaviour.Fleeing:
                animal.agent.speed = runSpeed;
                break;
            default:
                animal.agent.speed = baseSpeed;
                break;
        }
    }
}
