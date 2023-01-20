using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Transform mainCam;
    public CharacterController controller;
    public Transform groundCheck;

    public LayerMask groundMask;

    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public bool isGrounded;
    public float gravity = -9.81f * 2f;

    float turnSmoothVelocity;
    private float ySpeed;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        ySpeed += gravity * Time.deltaTime;

        if (isGrounded && ySpeed < 0)
        {
            ySpeed = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
       
        if (direction.magnitude >= 0.1f) // If there is some direction input
        {
            // Calculating desired angle for character to face forward
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;

            // Smooths turning angle so the target angle is reached in turnSmoothTime seconds
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 movement = speed * Time.deltaTime * moveDir.normalized;
            controller.Move(movement);
        }

        Debug.Log(isGrounded);
        controller.Move(new Vector3(0f, ySpeed, 0f) * Time.deltaTime);
    }
}
