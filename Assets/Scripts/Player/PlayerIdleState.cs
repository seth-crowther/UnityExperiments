using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        // Reset hover time when player is grounded
        player.GetHoverState().SetHoverComplete(false);
        player.GetHoverState().SetElapsedHoverTime(0f);
        player.GetAnimator().SetBool("isMoving", false);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);

        // If the player isn't grounded, default to the falling state
        if (!player.isGrounded)
        {
            player.SetYSpeed(0f);
            player.ChangeState(PlayerStateManager.PlayerState.fallingState);
        }

        if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            player.ChangeState(PlayerStateManager.PlayerState.jetpackState);
        }

        // If movement input detected, enter moving state
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            player.ChangeState(PlayerStateManager.PlayerState.movingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }
}
