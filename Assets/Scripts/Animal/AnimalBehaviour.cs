
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

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

[RequireComponent(typeof(AnimalStatus))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(Foraging))]
[RequireComponent(typeof(Fighting))]
[RequireComponent(typeof(Fleeing))]
[RequireComponent(typeof(CheckForEnemy))]
[RequireComponent(typeof(SpeedBehaviour))]

public class AnimalBehaviour : MonoBehaviour
{
    private Behaviour behaviour;
    private Target targetType;
    private Transform target;

    private NavMeshAgent agent;
    private AnimalStatus status;
    private Idle idle;
    private Foraging forage;
    private Fighting fight;
    private Fleeing flee;
    private CheckForEnemy enemyCheck;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<AnimalStatus>();
        idle = GetComponent<Idle>();
        forage = GetComponent<Foraging>();
        fight = GetComponent<Fighting>();
        flee = GetComponent<Fleeing>();
        enemyCheck = GetComponent<CheckForEnemy>();

        behaviour = Behaviour.Wandering;

        DebugBehaviour(transform.localScale.magnitude.ToString());
    }

 

  

    private void Update()
    {
        Behaviour lastBehaviour = behaviour;
        Transform lastTarget = target;

        enemyCheck.Search(); //keeping searching behaviour on always (???)

        if (target == null)
        {
            targetType  = Target.None;
        }

        switch (targetType)
        {
            case Target.None:
                if (status.hunger >= status.minHunger) //wanders if not hungry
                {
                    behaviour = Behaviour.Wandering;
                }
                else   //searches for food if hungry
                {
                    behaviour = Behaviour.SearchingForFood;
                }
                break;
            case Target.Food:
                behaviour = Behaviour.Eating; //investigate if new food target
                break;
            case Target.Prey:
                behaviour = Behaviour.Fighting; //chase if new prey target
                break;
            case Target.Threat:
                if (status.health < status.minHealth)
                {
                    DebugBehaviour("Self defense activated");
                    targetType = Target.Prey;
                    
                }
                else
                {
                    behaviour = Behaviour.Fleeing;
                } //flee if threat detected 
                                               //put some conditions down for self-defense!
                break;
        }

        switch (behaviour) //continuosly updated behaviour
        {
            case Behaviour.Wandering:
                idle.Wander();
                break;
            case Behaviour.SearchingForFood:
                forage.Search();
                break;
            case Behaviour.Eating:
                forage.GoEatFood();
                break;
            case Behaviour.Fighting:
                fight.Attack();
                break;
            case Behaviour.Fleeing:
                flee.Flee(); 
                break;
            
        }

        if (behaviour != lastBehaviour)
        {
            DebugBehaviour(behaviour.ToString());
        }
        if (target != null)
        {
            if (target != lastTarget)
            {
                DebugBehaviour($"New target - {target.name} ({targetType.ToString()})");
            }
        }
        
    }



    public Behaviour GetBehaviour()
    {
        return behaviour;
    }

    public Target GetTargetType()
    {
        return targetType;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetTarget(Transform target, Target type)
    {
        this.target = target;
        targetType = type;
    }

    public void RemoveTarget()
    {
        target = null;
        targetType = Target.None;
    }
 
    void DebugBehaviour(string s)
    {
        
        Debug.Log($"{status.stats.diet}: " + s);
        
    }

    public void KillAnimal()
    {
        Destructor();
    }

    private void Destructor()
    {
        Destroy(this);
        Destroy(agent);
        Destroy(status);
        IBehaviour[] behaveModules;
        behaveModules = GetComponents<IBehaviour>();
        foreach (IBehaviour module in behaveModules)
        {
            Destroy((Object)module);
        }
        gameObject.AddComponent<Rigidbody>();
        CorpseBehaviour corpseComponent = gameObject.AddComponent<CorpseBehaviour>();
        corpseComponent.stats = status.stats;
        name += (" (Dead)");
        gameObject.tag = "Meat";
    }

}
