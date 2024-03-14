using System.Collections.Generic;
using UnityEngine;

public class DiscerningSight : Sight
{
    [SerializeField] List<Transform> threatList = new List<Transform>();
    [SerializeField] List<Transform> foodList = new List<Transform>();
    [SerializeField] List<Transform> preyList = new List<Transform>();
    [SerializeField] List<Transform> friendList = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
        visibleUpdate += UpdateLists;
    }

    void ClearLists()
    {
        threatList.Clear();
        foodList.Clear();
        preyList.Clear();
        friendList.Clear();
    }

    void UpdateLists()
    {
        ClearLists();
        foreach (Transform t in visibleAnimals)
        {
            if (IsObjectThreat(t))
            {
                threatList.Add(t);
            }
        }
    }

  
    bool IsObjectThreat(Transform t)
    {
        Animal otherStats = t.GetComponent<Stats>().stats;
        if (otherStats.diet != Diet.Herbavore)
        {
            return true; //also need to check size!
        }
        return false;
    }

    //bool IsObjectFood(Transform t)
    //{
    //   if (stats.diet == Diet.Herbavore && t.
    //}
}