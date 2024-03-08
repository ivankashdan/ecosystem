using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGrowth : MonoBehaviour
{

    [SerializeField] List<Animal> lifeCycle;
    [HideInInspector] public Animal stats;
    [SerializeField] GameObject deadState;
    int lifeCycleStage = 0;

    float growRate = 10f;
    private void Start()
    {
        stats = lifeCycle[lifeCycleStage];
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        while (true)
        {
            if (stats != lifeCycle[lifeCycle.Count - 1])
            {
                stats = lifeCycle[lifeCycleStage++];
            }
            yield return new WaitForSeconds(growRate);
        }
    }

    void UpdateModel() //with animation?
    {

    }

}
