using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagingCarnivore : Foraging
{

    public override void SearchForFood()
    {
        List<Transform> foodList;


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
        ConsumableBehaviour corpse = target.GetComponent<ConsumableBehaviour>();
        status.hunger += corpse.age.nutrition;
        Destroy(corpse.gameObject);
    }
}
