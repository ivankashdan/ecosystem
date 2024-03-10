using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGrowth : BaseBehaviour
{

    [HideInInspector] public float growRate = 2f;
    [HideInInspector] public Animal stats;
    int lifeCycleStage = 0;

   

    public AnimalGrowth(ref AnimalBehaviour animal ) : base(ref animal)
    {
        stats = animal.lifeCycle.lifeCycle[lifeCycleStage];
        animal.StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        while (lifeCycleStage != animal.lifeCycle.lifeCycle.Count)
        {
            stats = animal.lifeCycle.lifeCycle[lifeCycleStage++];
            animal.UpdateModel();
            Debug.Log("Grew!");
            yield return new WaitForSeconds(growRate);
        }
    }

  

}
