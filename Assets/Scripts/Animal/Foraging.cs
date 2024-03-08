using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent (typeof(Idle))]
public abstract class Foraging : SearchBehaviour
{

    //emptyFunction eating;
    //public delegate void emptyFunction();
    //public event emptyFunction eating;


    [SerializeField] float searchWait = 2.0f;
    float searchTimePassed = 0;

    Idle idle;

    void Start()
    {
        idle = GetComponent<Idle>();
    }

    public void Search()
    {
        searchTimePassed += Time.deltaTime;

        if (searchTimePassed > searchWait)
        {
            searchTimePassed = 0;

            SearchForFood();

            idle.ChooseRandomDestination();
        }
    }

    public void GoEatFood()
    {
        Transform target = animal.GetTarget();
        if (target != null)
        {
            agent.destination = animal.GetTarget().position;

            if (agent.remainingDistance < status.stats.attackRange) //collide if inside collider
            {
                EatFood(target);

            }
        }
    }

    protected virtual void EatFood(Transform target) { }

    public virtual void SearchForFood() { }

    protected List<Transform> checkForEdiblePlants(List<Transform> foodList)
    {
        if (foodList != null)
        {
            List<Transform> ediblePlants = new List<Transform>();
            foreach (Transform plant in foodList)
            {
                if (plant.GetComponent<PlantBehaviour>().stats.edible)
                {
                    ediblePlants.Add(plant.transform);
                }
            }
            if (ediblePlants.Count > 0)
            {
                return ediblePlants;
            }
        }

        return null;
    }




}
