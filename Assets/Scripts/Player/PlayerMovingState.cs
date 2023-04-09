using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{

    public override void EnterState(PlayerStateManager player)
    {
        player.ySpeed = -20f;

        // Reset hover time when player is grounded
        player.hoverState.SetHoverComplete(false);
        player.hoverState.SetElapsedHoverTime(0f);
    }

    public override void UpdateState(PlayerStateManager player)
    {
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

        base.UpdateState(player);

        // Adjusting players y velocity based on 
        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }
}
