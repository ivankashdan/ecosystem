using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehaviour : LifeformBehaviour
{
    NavMeshAgent agent;

    [SerializeField] int _health;
    public int health
    {
        get { return _health; }
        set
        {
            if (_health > value)
            {
                StartCoroutine(FlashRed());
            }
            if (value > age.maxHealth)
            {
                _health = age.maxHealth;
            }
            else if (value <= 0)
            {
                _health = 0;

                Died();
            }
            else
            {
                _health = value;
            }
        }
    }

    [SerializeField] int _hunger;
    public int hunger
    {
        get { return _hunger; }
        set
        {
            if (value > age.maxHunger)
            {
                _hunger = age.maxHunger;
            }
            else if (value <= 0)
            {
                _hunger = 0;

                if (starvingCoroutine == null)
                {
                    starvingCoroutine = StartCoroutine(HealthPerSecond());
                }
            }
            else
            {
                _hunger = value;
            }
        }
    }


    int _thirst;
    public float thirst { get; set; }

    bool hungry = false;

    public int minHunger = 700;
    public int hungerPerSecond = 10;
    public int healthPerSecond = 10;

    public int wanderInterval = 2;
    public int waitToEat = 2;

    public float searchRadius = 6f;
    public int searchInterval = 3;

    public int lifeCycleStage;
    public List<Animal> lifeCycle;
    public GameObject deadState;
    public Animal age;

    bool edible = false;
    
    Coroutine doDamageCoroutine;
    Coroutine starvingCoroutine;

    //public delegate void animalDied();
    //public event animalDied AnimalDied;

    

    IEnumerator FlashRed()
    {
        Renderer renderer = transform.GetComponent<Renderer>();
        Color original = renderer.material.color;
        renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        renderer.material.color = original;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        age = lifeCycle[lifeCycleStage];

        edible = age.edible;
        health = age.maxHealth;
        hunger = 800; //temporary, set back to max...
        thirst = age.maxThirst;

        StartCoroutine(HungerPerSecond());
        StartCoroutine(Wander());
    }

    IEnumerator HungerPerSecond()
    {
        while(true)
        {
            hunger -= hungerPerSecond;

            if (hunger < minHunger)
            {
                hungry = true;
            }
            else if (hunger >= minHunger)
            {
                hungry = false;
            }
                
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator HealthPerSecond()
    {
        while (hunger <= 0)
        {
            health -= healthPerSecond;

            yield return new WaitForSeconds(1);
        }
        StopCoroutine(starvingCoroutine);
        starvingCoroutine = null;
    }

    IEnumerator Wander()
    {
        bool wandering = true;

        while (wandering)
        {
            if (hungry)
            {
                StartCoroutine(SearchForFood());
                wandering = false;
            }
            else
            {
                
                RandomDestination();
            }

            yield return new WaitForSeconds(wanderInterval);
        }

    }

    IEnumerator SearchForFood()
    {
        bool searching = true;

        if (hungry)
        {
            while (searching)
            {
                DebugCarnivore("Searching...");

                Transform target = Search();

                if (target != null)
                {
                    if (target.CompareTag("Plant"))
                    {
                        StartCoroutine(GoEatFood(target));
                    }
                    else if (target.CompareTag("Animal")) //replace with tag checks
                    {
                        if (target.GetComponent<ConsumableBehaviour>())
                        {
                            StartCoroutine(GoEatFood(target));
                        }
                        else if (target.GetComponent<AnimalBehaviour>())
                        {
                            StartCoroutine(AttackTarget(target));
                        }
                    }

                    searching = false;
                    yield break;

                }
                else
                {
                    RandomDestination();
                }

                yield return new WaitForSeconds(searchInterval);
            }
        }
        else
        {
            StartCoroutine(Wander());
        }
    }

    void RandomDestination()
    {
        DebugCarnivore("Wandering...");
        //agent.destination = transform.position;
        agent.destination = transform.position + (Random.insideUnitSphere * searchRadius);
    }
  

    private Transform Search()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius); //search all colliders in sphere

        List<Transform> hitEdibles = new List<Transform>();

        foreach (Collider collider in hitColliders)
        {
            if (collider.transform != this.transform) //check it's not self!
            {

                if (age.diet == Diet.Herbavore || age.diet == Diet.Omnivore)
                {
                    if (collider.CompareTag("Plant"))
                    {
                        if (collider.GetComponent<PlantBehaviour>().age.edible)
                        {
                            hitEdibles.Add(collider.transform);
                        }
                    }
                }
                if (age.diet == Diet.Carnivore || age.diet == Diet.Omnivore)
                {
                    if (collider.CompareTag("Animal"))
                    {
                        hitEdibles.Add(collider.transform);
                    }
                }
            }
        }

        if (hitEdibles.Count == 0)
        {
            return null;
        }

        return FindClosest(hitEdibles);

    }

    Transform FindClosest(List<Transform> hitEdibles)
    {
        float closestDistance = searchRadius;
        Transform closestTarget = null;

        foreach (Transform edible in hitEdibles)
        {
            float distance = Vector3.Distance(edible.position, this.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = edible;
            }

        }
        if (closestTarget != null)
        {
            return closestTarget.transform;
        }
        else
        {
            return null;
        }

    }

    IEnumerator AttackTarget(Transform target)
    {
        DebugCarnivore("Attacking " + target.name);

        AnimalBehaviour prey = target.GetComponent<AnimalBehaviour>();

        doDamageCoroutine = StartCoroutine(DamageTarget(target));


        while (prey.health > 0) //continually update enemy position
        {
            agent.destination = target.position; //keep attacking

            yield return new WaitForSeconds(1);
        }

        StartCoroutine(SearchForFood());
        StopCoroutine(doDamageCoroutine);
    }

    IEnumerator DamageTarget(Transform target)
    {
        AnimalBehaviour prey = target.GetComponent<AnimalBehaviour>();

        while (prey.health > 0)
        {
            if (agent.remainingDistance < age.attackRange) //collide if inside collider
            {

                DebugCarnivore("Damaging " + target.name);
                prey.health -= age.attackStrength;

                yield return new WaitForSeconds(age.attackDPS);
            }
        }
    }

    IEnumerator GoEatFood(Transform target)
    {
        bool goingToEat = true;

        if (hungry)
        {
            DebugCarnivore("Going to eat " + target.name);

            while (goingToEat)
            {
                agent.destination = target.position;

                if (agent.remainingDistance < age.attackRange) //collide if inside collider
                {
                    StartCoroutine(EatFood(target));
                    goingToEat = false;
                    yield return new WaitForSeconds(waitToEat);
                }
                yield return new WaitForSeconds(1);
            }
        }
        StartCoroutine(Wander());
    }


    IEnumerator EatFood(Transform target)
    {
        DebugCarnivore("Ate " + target.name);

        if (target.CompareTag("Animal"))
        {
            yield return new WaitForSeconds(waitToEat);
            ConsumableBehaviour corpse = target.GetComponent<ConsumableBehaviour>();
            hunger += corpse.age.nutrition;
            Destroy(corpse.gameObject);
        }
        else if (target.CompareTag("Plant"))
        {
            yield return new WaitForSeconds(waitToEat);
            PlantBehaviour food = target.GetComponent<PlantBehaviour>();
            hunger += food.age.nutrition;
            food.Eaten();
        }
    }

    public void Died() //temporary code //or better yet, instantiate a dead bean
    {
        //AnimalDied();
        Destroy(gameObject);
        GameObject corpse = Instantiate(deadState, transform.position, transform.rotation, transform.parent);
        ConsumableBehaviour consumabale = corpse.GetComponent<ConsumableBehaviour>();
        consumabale.age = age;
        corpse.name = consumabale.age.objectName + (" (Dead)");
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    void DebugCarnivore(string s)
    {
        if (age.diet == Diet.Carnivore)
        {
            Debug.Log("Carnivore: " + s);
        }
    }




}
