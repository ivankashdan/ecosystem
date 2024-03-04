using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBehaviour : MonoBehaviour
{
    public Animal age;
    Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward, ForceMode.Impulse);
    }
}
