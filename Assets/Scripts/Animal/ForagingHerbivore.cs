using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagingHerbivore : Foraging
{
   

    public override void SearchForFood()
    {
        List<Transform> foodList;

        foodList = checkForEdiblePlants(SearchFor("Plant"));

        if (foodList != null)
        {
            animal.SetTarget(FindClosest(foodList), Target.Food);
        }
    }

    protected override void EatFood(Transform target)
    {
        PlantBehaviour food = target.GetComponent<PlantBehaviour>();
        status.hunger += food.stats.nutrition;
        food.Eaten();
        animal.RemoveTarget();

        //eating();

        if (GetComponent<Animator>()) //temporary!
        {
            GetComponent<Animator>().SetTrigger("Eating");
        }
    }
}
