

using UnityEngine;
using UnityEngine.AI;

public abstract class BaseBehaviour
{
    protected AnimalBehaviour animal;


    protected BaseBehaviour(ref AnimalBehaviour animal)
    {
        this.animal = animal;
    }

}