using System.Collections.Generic;
using UnityEngine;


public abstract class SearchBehaviour : BaseBehaviour
{
    protected SearchBehaviour(ref AnimalBehaviour animal) : base(ref animal) { }

    protected bool IsOtherBigger(Transform other)
    {
        float ownSize = animal.transform.localScale.magnitude;
        float otherSize = other.transform.localScale.magnitude;

        if (otherSize >= ownSize)
        {
            return true;
        }
        return false;
    }

    protected bool IsTargetStillVisible()
    {
        float distance = Vector3.Distance(animal.transform.position, animal.GetTarget().position);
        if (distance > animal.stats.searchRadius)
        {
            animal.RemoveTarget();
            return false;
        }
        return true;
    }

    protected Transform FindClosest(List<Transform> hitList)
    {
        float closestDistance = animal.stats.searchRadius;
        Transform closestTarget = null;

        foreach (Transform hit in hitList)
        {
            float distance = Vector3.Distance(hit.position, animal.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = hit;
            }

        }
        //Debug.Log($"closest target to {transform.name} is: {closestTarget.transform}");

        if (closestTarget != null)
        {
            return closestTarget.transform;
        }
        else { return null; }
    }

    protected List<Transform> SearchFor(string tag)
    {
        Collider[] colliders = SurroundingObjects();

        if (colliders != null)
        {
            List<Transform> tagList = new List<Transform>();

            foreach (Collider collider in colliders)
            {
                if (collider.transform != animal.transform) //check it's not self!
                {
                    if (collider.CompareTag(tag))
                    {
                        tagList.Add(collider.transform);
                    }
                }
            }

            if (tagList.Count != 0) //if none found
            {
                return tagList;
            }
        }

        return null;
    }

    Collider[] SurroundingObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(animal.transform.position, animal.stats.searchRadius); //search all colliders in sphere

        if (hitColliders.Length > 0)
        {
            return hitColliders;
        }
        return null;
    }

   

}
