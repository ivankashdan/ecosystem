using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagingOmnivore : Foraging
{
    public ForagingOmnivore(ref AnimalBehaviour animal) : base(ref animal) { }

    public override void SearchForFood() //instead compile all this from other functions
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
            CorpseBehaviour corpse = target.GetComponent<CorpseBehaviour>();
            animal.status.ChangeHunger(corpse.stats.nutrition);
            corpse.RemoveCorpse();

        }
        else if (target.CompareTag("Plant"))
        {
            PlantBehaviour food = target.GetComponent<PlantBehaviour>();
            animal.status.ChangeHunger(food.stats.nutrition);
            food.Eaten();
            animal.RemoveTarget();
        }
    }
}
