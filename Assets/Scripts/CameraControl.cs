using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float scrollSensitivity = 20f;
    public float zoomSensitivity = 50f;


    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * scrollSensitivity;
        float deltaZ = Input.GetAxis("Vertical") * scrollSensitivity;
        float deltaY = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity; 

        Vector3 movement = new Vector3 (deltaX * Time.deltaTime, deltaY * Time.deltaTime, deltaZ * Time.deltaTime);
        //movement = Vector3.ClampMagnitude(movement, scrollSensitivity);


        transform.position += movement;



        
            
        
        

    }
}
