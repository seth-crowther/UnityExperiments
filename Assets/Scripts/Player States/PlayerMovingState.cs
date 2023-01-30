using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entering moving state");
        player.ySpeed = -20f;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (!player.isGrounded)
        {
            player.ChangeState(player.fallingState);
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                player.ChangeState(player.jetpackState);
            }
        }

        if (direction.magnitude >= 0.1f) // If there is some direction input
        {
            // Calculating desired angle for character to face forward
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + player.mainCam.eulerAngles.y;

            // Smooths turning angle so the target angle is reached in turnSmoothTime seconds
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref player.turnSmoothVelocity, player.turnSmoothTime);

            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 movement = player.walkingSpeed * Time.deltaTime * moveDir.normalized;
            player.controller.Move(movement);
        }

        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);
    }
}
