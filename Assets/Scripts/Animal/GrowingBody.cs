using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GrowingBody : Body
{
    [SerializeField] List<Animal> lifeCycle;
    [HideInInspector] int growRate = 10;
    int lifeCycleStage = 0;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Grow());

        if (lifeCycle.Count == 0)
        {
            Debug.Log("lifeCycle is not assigned!"); //Throw exception?
        }

    }

    IEnumerator Grow()
    {
        while (lifeCycleStage != lifeCycle.Count)
        {
            stats = lifeCycle[lifeCycleStage++];
            UpdateModel();
            Debug.Log("Grew!");
            yield return new WaitForSecondsRealtime(growRate);
        }
    }
}