using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(AnimalStatus))]
//[RequireComponent(typeof(AnimalBehaviour))]
public abstract class SearchBehaviour : MonoBehaviour
{

    protected NavMeshAgent agent;
    protected AnimalStatus status;
    protected AnimalBehaviour animal;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<AnimalStatus>();
        animal = GetComponent<AnimalBehaviour>();
    }

    protected void OnDrawGizmosSelected()
    {
        if (status!=null)
        {

            AnimalStatus status = GetComponent<AnimalStatus>();
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, status.stats.searchRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, status.stats.attackRange);
        }
    }

    protected bool IsOtherBigger(Transform other)
    {
        float ownSize = transform.localScale.magnitude;
        float otherSize = other.transform.localScale.magnitude;

        if (otherSize >= ownSize)
        {
            return true;
        }
        return false;
    }

    protected bool IsTargetStillVisible()
    {
        float distance = Vector3.Distance(transform.position, animal.GetTarget().position);
        if (distance > status.stats.searchRadius)
        {
            animal.RemoveTarget();
            return false;
        }
        return true;
    }

    protected Transform FindClosest(List<Transform> hitList)
    {
        float closestDistance = status.stats.searchRadius;
        Transform closestTarget = null;

        foreach (Transform hit in hitList)
        {
            float distance = Vector3.Distance(hit.position, this.transform.position);

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
                if (collider.transform != this.transform) //check it's not self!
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, status.stats.searchRadius); //search all colliders in sphere

        if (hitColliders.Length > 0)
        {
            return hitColliders;
        }
        return null;
    }

   

}
