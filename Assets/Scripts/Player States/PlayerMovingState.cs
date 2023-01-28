using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    // Constants
    public float speed = 16.0f;
    public float turnSmoothTime = 0.1f;
    public float jumpHeight = 10f;
    public float groundDistance = 1.0f;
    public float headDistance = 0.1f;
    public float gravity = -60f;

    // To be referenced
    public bool isGrounded;
    public bool hasHitHead;
    private float turnSmoothVelocity;

    public override void EnterState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {
        isGrounded = Physics.CheckSphere(player.groundCheck.position, groundDistance, player.groundMask, QueryTriggerInteraction.Ignore);
        hasHitHead = Physics.CheckSphere(player.headCheck.position, headDistance, player.groundMask, QueryTriggerInteraction.Ignore);

        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        player.ySpeed += gravity * Time.deltaTime;

        if (isGrounded && player.ySpeed < 0)
        {
            player.ySpeed = -4f;

            if (Input.GetButtonDown("Jump"))
            {
                // Vertical speed needed to reach a given height
                player.ySpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        if (hasHitHead)
        {
            player.ySpeed = -1f;
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

        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);
    }
}
