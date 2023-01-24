using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    // Constants
    public float speed = 8.0f;
    public float turnSmoothTime = 0.1f;
    public float jumpHeight = 10f;
    public float groundDistance = 1.0f;
    public float gravity = -60f;

    // To be referenced
    public bool isGrounded;
    private float turnSmoothVelocity;
    private float ySpeed;

    public override void EnterState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {
        isGrounded = Physics.CheckSphere(player.groundCheck.position, groundDistance, player.groundMask);

        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        ySpeed += gravity * Time.deltaTime;

        if (isGrounded && ySpeed < 0)
        {
            ySpeed = -4f;

            if (Input.GetButtonDown("Jump"))
            {
                // Vertical speed needed to reach a given height
                ySpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        if (direction.magnitude >= 0.1f) // If there is some direction input
        {
            // Calculating desired angle for character to face forward
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + player.mainCam.eulerAngles.y;

            // Smooths turning angle so the target angle is reached in turnSmoothTime seconds
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 movement = speed * Time.deltaTime * moveDir.normalized;
            player.controller.Move(movement);
        }

        player.controller.Move(new Vector3(0f, ySpeed, 0f) * Time.deltaTime);
    }
}
