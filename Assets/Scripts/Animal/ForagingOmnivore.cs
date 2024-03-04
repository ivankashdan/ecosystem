using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagingOmnivore : Foraging
{
    public override void SearchForFood()
    {
        List<Transform> foodList;

        foodList = checkForEdiblePlants(SearchFor("Plant"));

        if (foodList != null)
        {
            animal.SetTarget(FindClosest(foodList), Target.Food);
        }

        foodList = SearchFor("Meat");

        if (foodList != null)
        {
            animal.SetTarget(FindClosest(foodList), Target.Food);
        }
        else
        {
            foodList = SearchFor("Animal");

            if (foodList != null)
            {
                animal.SetTarget(FindClosest(foodList), Target.Prey);
            }
        }
    }

    protected override void EatFood(Transform target)
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
            animal.RemoveTarget();
        }
    }
}
