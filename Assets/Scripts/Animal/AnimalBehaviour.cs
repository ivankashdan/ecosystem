
using UnityEngine;
using UnityEngine.AI;

public enum Behaviour
{
    Wandering,
    SearchingForFood,
    Investigating, //change to Eating
    Chasing, //change to Fighting
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
[RequireComponent(typeof(CheckForEnemy))]

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
    private CheckForEnemy enemyCheck;


    //[SerializeField] public float searchRadius = 6.0f;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<AnimalStatus>();
        idle = GetComponent<Idle>();
        forage = GetComponent<Foraging>();
        fight = GetComponent<Fighting>();
        enemyCheck = GetComponent<CheckForEnemy>();

        behaviour = Behaviour.Wandering;
    }

    private void Update()
    {
        Behaviour lastBehaviour = behaviour;

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
                behaviour = Behaviour.Investigating; //investigate if new food target
                break;
            case Target.Prey:
                behaviour = Behaviour.Chasing; //chase if new prey target
                break;
            case Target.Threat:
                behaviour = Behaviour.Fleeing; //flee if threat detected 
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
            case Behaviour.Investigating:
                forage.GoEatFood();
                break;
            case Behaviour.Chasing:
                fight.Chase(); //Attack, strike?
                break;
            case Behaviour.Fleeing:
                fight.Flee(); 
                break;
            
        }

        if (behaviour != lastBehaviour)
        {
            DebugBehaviour(behaviour.ToString());
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




}
