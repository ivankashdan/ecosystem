using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;


public enum Behaviour
{
    Wandering,
    SearchingForFood,
    Eating,
    Fighting,
    Fleeing,
}

public enum Target
{
    None,
    Food,
    Prey,
    Threat,
}

public class Brain : MonoBehaviour
{

    Behaviour state;
    NavMeshAgent agent;
    Needs needs;
    Idle idle;
    Foraging forage;
    Fighting fight;
    Fleeing flee;
    CheckForEnemy enemyCheck;
    SpeedBehaviour speed;


    public void KillCount() //testing event
    {
        Debug.Log("Animal Killed...");
    }


    private void Awake()
    {
        AssignMods();   
    }


    void AssignMods()
    {
        needs = GetComponent<Needs>();
    }

    private void Update()
    {
        Behaviour lastBehaviour = state;

        switch (state) //continuosly updated behaviour
        {
            //case Behaviour.Wandering:
            //    idle.Wander();
            //    break;
            //case Behaviour.SearchingForFood:
            //    forage.Search();
            //    break;
            //case Behaviour.Eating:
            //    forage.GoEatFood();
            //    break;
            //case Behaviour.Fighting:
            //    fight.Attack();
            //    break;
            //case Behaviour.Fleeing:
            //    flee.Flee();
            //    break;
        }

    }

    

}
