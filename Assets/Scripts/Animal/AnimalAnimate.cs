using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(NavMeshAgent))]
public class AnimalAnimate : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        animator.SetFloat("Moving", agent.velocity.magnitude);

        //now it needs to 'listen for events: eaten, attacking, hurt, killed...
    }
}
