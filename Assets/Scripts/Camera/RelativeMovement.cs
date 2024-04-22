using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour
{

    [SerializeField] Transform target;
    public float rotSpeed = 15.0f;
    float moveSpeed = 1f;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    private CharacterController charController;

    public float jumpSpeed = 25f;//15.0f;
    public float gravity = -2;//-9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float vertSpeed;


    void Start()
    {
        charController = GetComponent<CharacterController>();
        vertSpeed = minFall;
    }
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }


        Vector3 movement = Vector3.zero;



        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            Vector3 right = target.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);
            movement = (right * horInput) + (forward * vertInput);

            movement *= moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed); //for diagonals

            //smooth movement
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);

            //transform.rotation = Quaternion.LookRotation(movement);
            //^original movement (instantaneous)
        }

        if (charController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }
        movement.y = vertSpeed;

        movement *= Time.deltaTime;
        charController.Move(movement);

   
    }
}
