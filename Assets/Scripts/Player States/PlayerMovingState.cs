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

        // If the player isn't grounded, default to the falling state
        if (!player.isGrounded)
        {
            player.ySpeed = 0f;
            player.ChangeState(player.fallingState);
        }

        // If the player is grounded, transition to jetpack state if the spacebar is pressed
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

            // Rotating player towards targetAngle slowly, and moving based on direction vector
            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 movement = player.walkingSpeed * Time.deltaTime * moveDir.normalized;
            player.controller.Move(movement);
        }

        // Adjusting players y velocity based on 
        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);
    }
}
