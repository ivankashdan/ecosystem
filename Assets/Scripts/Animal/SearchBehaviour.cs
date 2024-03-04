using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AnimalStatus))]
public abstract class SearchBehaviour : MonoBehaviour
{

    //[SerializeField] protected float searchRadius = 6f;

    protected AnimalStatus status;

    private void Awake()
    {
        status = GetComponent<AnimalStatus>();
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

    Collider[] SurroundingObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, status.stats.searchRadius); //search all colliders in sphere

        if (hitColliders.Length > 0)
        {
            return hitColliders;
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        if (status != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, status.stats.searchRadius);
        }
    }

}
