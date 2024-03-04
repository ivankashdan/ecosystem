using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AnimalStatus))]
[RequireComponent(typeof(NavMeshAgent))]
public class AnimalBehaviour : MonoBehaviour
{
    private enum Behaviour
    {
        Wandering,
        SearchingForFood,
        Investigating,
        Eating,
        Chasing,
        Fleeing,
        Fighting
    }

    [SerializeField] private Behaviour behaviour;
    private AnimalStatus status;
    private NavMeshAgent agent;
    float baseSpeed;
    float runSpeed;

    [SerializeField] private Transform target;
    [SerializeField] private Transform danger;

    private void Update()
    {
        Behaviour lastBehaviour = behaviour;

        if (target == null)
        {
            if (agent.speed == runSpeed)
            {
                agent.speed = baseSpeed;
            }
        }


        if (status.hunger >= status.minHunger)
        {
            behaviour = Behaviour.Wandering;
        }
        else if (target == null)
        {
            behaviour = Behaviour.SearchingForFood;
        }
        
        //if (danger == null)
        //{
        //    SearchPredator();
        //}

        switch (behaviour)
        {
            case Behaviour.Wandering:
                Wander();
                break;
            case Behaviour.SearchingForFood:
                Search();
                break;
            case Behaviour.Investigating:
                GoEatFood(target);
                break;
            case Behaviour.Eating:
                EatFood(target);
                break;
            case Behaviour.Chasing:
                Chase(target);
                break;
            case Behaviour.Fleeing:
                Flee(danger); 
                break;
            case Behaviour.Fighting:
                Attack(target);
                break;
        }

        if (behaviour != lastBehaviour)
        {
            DebugBehaviour(behaviour.ToString());
        }

    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<AnimalStatus>();
        behaviour = Behaviour.Wandering;
        baseSpeed = agent.speed * status.stats.walkSpeedMultiplier;
        runSpeed = agent.speed * status.stats.runSpeedMultiplier;
    }


    float wanderTimePassed = 0;
    float wanderWait = 5.0f;



    void Wander()
    {
        wanderTimePassed += Time.deltaTime;

        if (wanderTimePassed > wanderWait)
        {
            wanderTimePassed = 0;

            if (agent.remainingDistance < 0.1f)
            {
                agent.destination = transform.position + (Random.insideUnitSphere * searchRadius);
            }
        }

    }

    float searchTimePassed = 0;
    [SerializeField] float searchWait = 2.0f;
    [SerializeField] float searchRadius = 6f;
    void Search()
    {
        searchTimePassed += Time.deltaTime;

        if (searchTimePassed > searchWait)
        {
            searchTimePassed = 0;

            SearchForFood(status.stats.diet);

            if (target == null)
            {
                agent.destination = transform.position + (Random.insideUnitSphere * searchRadius);
            }
        }
    }

    float searchPredatorTimePassed = 0;
    [SerializeField] float searchPredatorWait = 5.0f;
    
    void SearchPredator()
    {
        searchPredatorTimePassed += Time.deltaTime;

        if (searchPredatorTimePassed > searchPredatorWait)
        {
            searchPredatorTimePassed = 0;

            SearchForPredator();
        }
    }

    void SearchForPredator()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius); //search all colliders in sphere

        if (hitColliders.Length > 0)
        {
            List<Transform> hitPredators = new List<Transform>();

            foreach (Collider collider in hitColliders)
            {
                if (collider.transform != this.transform) //check it's not self!
                {
                    if (collider.CompareTag("Animal"))
                    {
                        Diet diet = collider.GetComponent<AnimalStatus>().stats.diet;
                        if (diet == Diet.Carnivore || diet == Diet.Omnivore)
                        {
                            hitPredators.Add(collider.transform);
                        }
                    }
                }
            }

            if (hitPredators.Count != 0)
            {
                danger = FindClosest(hitPredators);

                if (danger != null)
                {
                    behaviour = Behaviour.Fleeing;
                }
            }
        }
    }

    void Flee(Transform danger)
    {
        target = null;

        if (agent.speed == baseSpeed)
        {
            agent.speed = runSpeed;
        }
        agent.destination -= danger.position*2;

        float distance = Vector3.Distance(transform.position, danger.position);

        if (distance > searchRadius) //collide if inside collider
        {
            this.danger = null;
        }
    }

    private void SearchForFood(Diet diet)
    {
       
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius); //search all colliders in sphere
       
        if (hitColliders.Length > 0)
        {
            List<Transform> hitEdibles = new List<Transform>();

            foreach (Collider collider in hitColliders)
            {
                if (collider.transform != this.transform) //check it's not self!
                {
                    if (diet == Diet.Herbavore || diet == Diet.Omnivore)
                    {
                        if (collider.CompareTag("Plant"))
                        {
                            if (collider.GetComponent<PlantBehaviour>().stats.edible)
                            {
                                hitEdibles.Add(collider.transform);
                            }
                        }
                    }

                    if (diet == Diet.Carnivore || diet == Diet.Omnivore)
                    {
                        if (collider.CompareTag("Meat"))
                        {
                            hitEdibles.Add(collider.transform);
                        }
                    }
                }
            }

            if (hitEdibles.Count != 0) //if none found
            {
                target = FindClosest(hitEdibles);
                behaviour = Behaviour.Investigating;
            }
            else if (diet == Diet.Carnivore || diet == Diet.Omnivore)
            {
                SearchForPrey(hitColliders);
            }
        }
        

    }

  

    void SearchForPrey(Collider[] hitColliders)
    {
        List<Transform> hitPrey = new List<Transform>();
        foreach (Collider collider in hitColliders)
        {
            if (collider.transform != this.transform) //check it's not self!
            {
                if (collider.CompareTag("Animal"))
                {
                    hitPrey.Add(collider.transform);
                }
            }
        }
        if (hitPrey.Count != 0)
        {
            target = FindClosest(hitPrey);
            behaviour = Behaviour.Chasing;
        }
    }

   

    Transform FindClosest(List<Transform> hitList)
    {
        float closestDistance = searchRadius;
        Transform closestTarget = null;

        foreach (Transform hit in hitList)
        {
            float distance = Vector3.Distance(hit.position, this.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = hit;
            }

        }
        //Debug.Log($"closest target to {transform.name} is: {closestTarget.transform}");
        
        if (closestTarget != null)
        {
            return closestTarget.transform;
        }
        else { return null; }   
    }

    void GoEatFood(Transform target)
    {
        agent.destination = target.position;

        if (agent.remainingDistance < status.stats.attackRange) //collide if inside collider
        {
            behaviour = Behaviour.Eating;
        }
    }

    void EatFood(Transform target)
    {

        if (target.CompareTag("Meat"))
        {
            ConsumableBehaviour corpse = target.GetComponent<ConsumableBehaviour>();
            status.hunger += corpse.age.nutrition;
            Destroy(corpse.gameObject);
            
        }
        else if (target.CompareTag("Plant"))
        {
            PlantBehaviour food = target.GetComponent<PlantBehaviour>();
            status.hunger += food.stats.nutrition;
            food.Eaten();
;       }
        this.target = null;
    }

    void Chase(Transform target)
    {
        if (agent.speed == baseSpeed)
        {
            agent.speed = runSpeed;
        }
        agent.destination = target.position;

        if (agent.remainingDistance < status.stats.attackRange) //collide if inside collider
        {
            behaviour = Behaviour.Fighting;
        }
    }

 

    float attackTimePassed = 0;
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
        else
        {
            behaviour = Behaviour.SearchingForFood;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    void DebugBehaviour(string s)
    {
        
        Debug.Log($"{status.stats.diet}: " + s);
        
    }




}
