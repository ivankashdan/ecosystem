using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagingCarnivore : Foraging
{
    public ForagingCarnivore(ref AnimalBehaviour animal) : base(ref animal) { }

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
            foodList = CheckForSmaller(SearchFor("Animal"));

            if (foodList != null)
            {
                animal.SetTarget(FindClosest(foodList), Target.Prey);
            }
        }
    }

    protected override void EatFood(Transform target)
    {
        CorpseBehaviour corpse = target.GetComponent<CorpseBehaviour>();
        animal.status.ChangeHunger(corpse.stats.nutrition);
        corpse.RemoveCorpse();
    }

    List<Transform> CheckForSmaller(List<Transform> foodList)
    {
        if (foodList != null)
        {
            List<Transform> smallerAnimals = new List<Transform>();
            foreach (Transform other in foodList)
            {
                if (!IsOtherBigger(other))
                {
                    smallerAnimals.Add(other.transform);
                }
            }
            if (smallerAnimals.Count > 0)
            {
                return smallerAnimals;
            }
        }
        return null;
    }

}
