using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

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
    
public class AnimalBehaviour : MonoBehaviour
{

    protected AnimalBehaviour animal;
    private Behaviour behaviour;
    private Target targetType;
    private Transform target;

    [SerializeField] public LifeCycle lifeCycle; //need to assign for now
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Renderer model;
    [HideInInspector] public AnimalStatus status;
    [HideInInspector] public AnimalGrowth growth;
    [HideInInspector] public Animal stats;
    [HideInInspector] public Idle idle;
    protected Foraging forage;
    protected Fighting fight;
    protected Fleeing flee;
    protected CheckForEnemy enemyCheck;
    protected SpeedBehaviour speed;

    private void Awake()
    {
        InitDefaultMods();
        InitGameObject();
        InitMods();

        behaviour = Behaviour.Wandering;

        DebugBehaviour(transform.localScale.magnitude.ToString()); //animal scale debug
    }

    private void InitDefaultMods() 
    {

        animal = this;
        growth = new AnimalGrowth(ref animal);
        stats = growth.stats;
        //model = transform.GetChild(0).GetComponent<Renderer>();
        status = new AnimalStatus(ref animal);
    }

    void InitGameObject()
    {
        gameObject.name = stats.objectName;
        gameObject.tag = "Animal";
        agent = gameObject.AddComponent<NavMeshAgent>();
    }

    void InitMods() 
    {
        idle = new Idle(ref animal);
        switch (stats.diet)
        {
            case Diet.Herbavore:
                forage = new ForagingHerbivore(ref animal);
                break;
            case Diet.Carnivore:
                forage = new ForagingCarnivore(ref animal);
                break;
            case Diet.Omnivore:
                forage = new ForagingOmnivore(ref animal);
                break;
        }
        fight = new Fighting(ref animal);
        flee = new Fleeing(ref animal);
        enemyCheck = new CheckForEnemy(ref animal);
        speed = new SpeedBehaviour(ref animal);
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

    protected void OnDrawGizmosSelected()
    {
        if (status != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, stats.searchRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.attackRange);
        }
    }

    public void UpdateModel()
    {
        //if (transform.GetChild(0))
        //{
        //    GameObject oldModel = transform.GetChild(0).gameObject;
        //    Destroy(oldModel.gameObject);
        //}
        //GameObject newModel = Instantiate(stats.model.gameObject, transform);
        //newModel.name = stats.name;
        //model = newModel.GetComponent<Renderer>();
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
        
        Debug.Log($"{stats.diet}: " + s);
        
    }

    public void KillAnimal()
    {
        Destructor();
    }

    private void Destructor()
    {
        gameObject.AddComponent<Rigidbody>();
        CorpseBehaviour corpseComponent = gameObject.AddComponent<CorpseBehaviour>();
        corpseComponent.stats = stats; 
        model.material.color = status.original;
        name += (" (Dead)");
        gameObject.tag = "Meat";
        Destroy(agent);
        Destroy(this);
    }

}
