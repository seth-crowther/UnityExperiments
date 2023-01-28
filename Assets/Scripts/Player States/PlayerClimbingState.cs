using UnityEngine;

public class PlayerClimbingState : PlayerBaseState
{
    public Transform transform;
    private Transform climbing;

    public float climbingSpeed = 5.0f;

    public float speed = 8.0f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public override void EnterState(PlayerStateManager player)
    {
        climbing = player.climbing;
        player.ySpeed = 0f;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

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
            player.controller.Move(climbingSpeed * Time.deltaTime * Vector3.Cross(-player.transform.forward, climbing.right).normalized);
        }
    }
}
