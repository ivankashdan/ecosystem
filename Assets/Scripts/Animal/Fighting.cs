using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(AnimalBehaviour))]
[RequireComponent (typeof(AnimalStatus))]
public class Fighting : MonoBehaviour
{
    NavMeshAgent agent;
    AnimalBehaviour animal;
    AnimalStatus status;

    [SerializeField] float fleeDistanceMultiplier = 5.0f;
    //[SerializeField] float fightRadius = 6.0f;

    float attackTimePassed = 0;
    float baseSpeed;
    float runSpeed;
    bool fleeDestinationSet = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animal = GetComponent<AnimalBehaviour>();   
        status = GetComponent<AnimalStatus>();

        baseSpeed = agent.speed * status.stats.walkSpeedMultiplier;
        runSpeed = agent.speed * status.stats.runSpeedMultiplier;
    }


    void Update() //handle speed 
    {
        switch (animal.GetBehaviour())
        {
            case Behaviour.Chasing:
            case Behaviour.Fleeing:
                agent.speed = runSpeed;
                break;
            default:
                agent.speed = baseSpeed;
                break;
        }
    }

    public void Chase()
    {
        if (IsTargetStillVisible())
        {
            Transform target = animal.GetTarget();
            agent.destination = target.position;

            if (agent.remainingDistance < status.stats.attackRange) //collide if inside collider
            {
                Attack(target);
            }
        }
    }

    void Attack(Transform target)
    {
        attackTimePassed += Time.deltaTime;

        AnimalStatus prey = target.GetComponent<AnimalStatus>();
        if (prey.health > 0) //continually update enemy position
        {
            agent.destination = target.position; //keep attacking
            if (attackTimePassed > status.stats.attackDPS)
            {
                attackTimePassed = 0;
                //Debug.Log($"Damage = {status.stats.attackStrength}");
                prey.health -= status.stats.attackStrength;
            }
        }
        
    }

    public void Flee() 
        //improve this, so it finds a new destination periodically rather than constantly
    {
        if (IsTargetStillVisible())
        {
            Transform target = animal.GetTarget();
            if (!fleeDestinationSet)
            {
                agent.destination = -target.position * fleeDistanceMultiplier;
                fleeDestinationSet = true;
            }

            if (agent.remainingDistance < 2.0f) //this code necessary?
            {
                fleeDestinationSet = false;
            }
        }
        else
        {
            fleeDestinationSet = false;
        }
    }

    bool IsTargetStillVisible()
    {
        float distance = Vector3.Distance(transform.position, animal.GetTarget().position);
        if (distance > status.stats.searchRadius)
        {
            animal.RemoveTarget();
            return false;
        }
        return true;
    }



}
