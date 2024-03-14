using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Sight : MonoBehaviour
{
    Animal stats;
    protected List <Transform> visibleAnimals;
    float sightDelay = 2.0f;
    protected delegate void Del();
    protected event Del visibleUpdate;

    protected virtual void Awake()
    {
        stats = GetComponent<Stats>().stats;
        StartCoroutine(See());
    }

    IEnumerator See()
    {
        while (true)
        {
            visibleAnimals = GetVisibleColliders();
            visibleUpdate();
            yield return new WaitForSecondsRealtime(sightDelay); //need to find better way of delaying this...
        }
    }

    List<Transform> GetVisibleColliders()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.searchRadius); //search all colliders in sphere

        List<Transform> list = new List<Transform>();
        foreach (Collider collider in colliders)
        {
            if (IsObjectAnimal(collider.transform)) 
            {
                Transform gfxTransform = collider.transform; 

                if (!IsObjectSelf(gfxTransform))
                {
                    list.Add(GetAnimalTransform(gfxTransform)); //adds the parent Transform for the animal
                }
            }
        }
        return list;
    }

    bool IsObjectAnimal(Transform t)
    {
        if (GetAnimalTransform(t) != null)
        {
            return true;
        }
        return false;
    }

    bool IsObjectSelf(Transform t)
    {
        Body ownBody = GetComponent<Body>();
        if (t == ownBody.gfxCollider.transform) //discount self //need to discount ground too!
        {
            return true;
        }
        return false;
    }

    Transform GetAnimalTransform(Transform gfx)
    {
        return gfx.transform.GetComponentInParent<Stats>().transform; //this is causing a problem for some reason...
    }


}