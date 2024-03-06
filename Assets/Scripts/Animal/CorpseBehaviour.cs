using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseBehaviour : MonoBehaviour
{
    Rigidbody rb;
    [HideInInspector] public Animal stats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward, ForceMode.Impulse);
    }
}
