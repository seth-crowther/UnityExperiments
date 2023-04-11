using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{

    public override void EnterState(PlayerStateManager player)
    {
        player.ySpeed = -20f;
        player.animator.Play("walking");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.GetShootingState())
        {
            player.animator.Play("walkingAiming");
        }
        else
        {
            player.animator.Play("walking");
        }

        base.UpdateState(player);

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
            player.ChangeState(player.idleState);
        }

        // Adjusting players y velocity based on 
        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }
}
