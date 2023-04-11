using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    private float timeNotMoving = 0f;
    private float timeUntilIdle = 0.05f;

    public override void EnterState(PlayerStateManager player)
    {
        player.ySpeed = -20f;
        player.animator.SetBool("isMoving", true);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);

        CalculateAnimation(player);

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

        // If no direction buttons pressed, enter idle state
        // Important it's a single "&" so both statements are evaluated
        if (Input.GetAxisRaw("Horizontal") == 0f & Input.GetAxisRaw("Vertical") == 0f)
        {
            timeNotMoving += Time.deltaTime;
            if (timeNotMoving > timeUntilIdle)
            {
                player.ChangeState(player.idleState);
            }
        }
        else
        {
            timeNotMoving = 0f;
        }

        // Adjusting players y velocity based on 
        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public void CalculateAnimation(PlayerStateManager player)
    {
        if (player.GetShootingState())
        {
            if (player.inputDirection.x > 0)
            {
                player.animator.SetInteger("strafe", 2);
            }
            else if (player.inputDirection.x < 0)
            {
                player.animator.SetInteger("strafe", 0);
            }
            else
            {
                player.animator.SetInteger("strafe", 1);
            }
        }
    }
}
