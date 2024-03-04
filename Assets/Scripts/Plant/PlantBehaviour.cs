using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlantBehaviour : LifeformBehaviour
{

    public int lifeCycleStage;
    public List<Plant> lifeCycle;
    public Plant stats;

   

    private void Start()
    {
        stats = lifeCycle[lifeCycleStage];

        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        while (true)
        {
            if (lifeCycleStage != lifeCycle.Count - 1)
            {
                //Debug.Log("Grow...");
                UpdateHeight(lifeCycleStage + 1);
            }
            yield return new WaitForSeconds(growRate);
        }
    }

    public void Eaten()
    {
        UpdateHeight(0);
    }

    public void UpdateHeight(int stage)
    {

        lifeCycleStage = stage;
        stats = lifeCycle[lifeCycleStage]; //need to make this automatic

        Vector3 scale = transform.localScale;
        switch (stage)
        {
            case 0:
                transform.localScale = new Vector3(scale.x, 0.1f, scale.z);
                break;
            case 1:
                transform.localScale = new Vector3(scale.x, 0.5f, scale.z);
                break;
            case 2:
                transform.localScale = new Vector3(scale.x, 2f, scale.z);
                break;
        }
        //reposition model
        Vector3 pos = transform.localPosition;
        transform.localPosition = new Vector3(pos.x, transform.localScale.y / 2, pos.z);

    }

  




}
