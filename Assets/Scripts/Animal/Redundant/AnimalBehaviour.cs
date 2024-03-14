using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

//public enum Behaviour
//{
//    Wandering,
//    SearchingForFood,
//    Eating,
//    Fighting,
//    Fleeing,
//}

//public enum Target
//{
//    None,
//    Food,
//    Prey,
//    Threat,
//}
    
public class AnimalBehaviour : MonoBehaviour
{

    protected AnimalBehaviour animal;
    private Behaviour behaviour;
    private Target targetType;
    private Transform target;

    GameObject model;
    [SerializeField] List<Animal> lifeCycle; //need to assign for now
    [HideInInspector] public Animal stats;
    [HideInInspector] int growRate = 10;
    int lifeCycleStage = 0;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AnimalStatus status;
    [HideInInspector] public Idle idle;
    protected Foraging forage;
    protected Fighting fight;
    protected Fleeing flee;
    protected CheckForEnemy enemyCheck;
    protected SpeedBehaviour speed;

    private void Awake()
    {
        InitGrowth();
        InitDefaultMods();
        InitGameObject();
        InitMods();

        behaviour = Behaviour.Wandering;

        DebugBehaviour(transform.localScale.magnitude.ToString()); //animal scale debug
    }

    private void InitGrowth()
    {
        if (transform.GetChild(0))
        {
            model = transform.GetChild(0).gameObject;
        }

        stats = lifeCycle[lifeCycleStage];
        StartCoroutine(Grow());
    }

    private void InitDefaultMods() 
    {
        animal = this;
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

        status.Needs();
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

    IEnumerator Grow()
    {
        while (lifeCycleStage != lifeCycle.Count)
        {
            stats = lifeCycle[lifeCycleStage++];
            UpdateModel();
            Debug.Log("Grew!");
            yield return new WaitForSecondsRealtime(growRate);
        }
    }


    public void UpdateModel()
    {
        //if (model)
        //{
        //    Destroy(model.gameObject);
        //}

        //model = Instantiate(stats.model, transform);
        //model.name = "GFX";
        //name = stats.objectName;
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
        //model.material.color = status.original;
        name += (" (Dead)");
        gameObject.tag = "Meat";
        Destroy(agent);
        Destroy(this);
    }

   
}
