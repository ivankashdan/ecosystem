using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SeedField : MonoBehaviour
{
    public GameObject grain;

    public int rowLength = 9;
    public int columnLength = 9;
    public int interval = 3;


    private void Start()
    {
        Seed();
    }

    void Seed()
    {
        Debug.Log("Field seeded...");
        Clear();

        for (int c = 0; c < rowLength; c++)  //seed from top left 
        {
            for (int r = 0; r < columnLength; r++)
            {
                GameObject newGrain = Instantiate(grain, this.transform.position + new Vector3(c * interval, 0, r * interval), Quaternion.identity, this.transform);
                newGrain.name = "Grain";
            }
        }
    }
    void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }




}
