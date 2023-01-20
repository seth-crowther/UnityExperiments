using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Transform mainCam;
    public CharacterController controller;

    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public float jumpSpeed = 0.5f;

    float turnSmoothVelocity;
    private float distToGround;
    private float ySpeed;

    private void Start()
    {
        distToGround = controller.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

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
    }

    public bool IsGrounded() // Doesn't necessarily work, what if capsule is sitting on a small hole. Fix later.
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
