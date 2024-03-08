
using System.Collections.Generic;
using UnityEngine;

public class CheckForEnemy : SearchBehaviour, IBehaviour
{
    [SerializeField] float searchPredatorWait = 1.0f;
    float searchPredatorTimePassed = 0;

    public void Search()
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
        List<Transform> enemyList;
        enemyList = checkForPredators(SearchFor("Animal"));

        if (enemyList != null)
        {
            animal.SetTarget(FindClosest(enemyList), Target.Threat);
        }
    }

    List<Transform> checkForPredators(List<Transform> animalList)
    {
        if (animalList != null)
        {
            List<Transform> predators = new List<Transform>();
            foreach (Transform other in animalList)
            {
                if (IsOtherBigger(other) && IsOtherPredator(other))
                {
                    predators.Add(other.transform);
                }
            }
            if (predators.Count > 0)
            {
                return predators;
            }
        }
        return null;
    }

 

    bool IsOtherPredator(Transform other)
    {
        Diet otherDiet = other.GetComponent<AnimalStatus>().stats.diet;
        if (otherDiet == Diet.Carnivore || otherDiet == Diet.Omnivore)
        {
            return true;
        }
        return false;
    }
    
}
