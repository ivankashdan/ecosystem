using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagingHerbivore : Foraging
{

    public ForagingHerbivore(ref AnimalBehaviour animal) : base(ref animal) { }


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
        animal.status.ChangeHunger(food.stats.nutrition);
        food.Eaten();
        animal.RemoveTarget();

        //eating(); //need an eating event here!

        //if (GetComponent<Animator>()) //temporary!
        //{
        //    GetComponent<Animator>().SetTrigger("Eating");
        //}
    }
}
