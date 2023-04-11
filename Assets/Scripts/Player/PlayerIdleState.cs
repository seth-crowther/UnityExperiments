using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        // Reset hover time when player is grounded
        player.hoverState.SetHoverComplete(false);
        player.hoverState.SetElapsedHoverTime(0f);
        player.animator.Play("idle");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);

        if (player.GetShootingState())
        {
            player.animator.Play("idleAiming");
        }
        else
        {
            player.animator.Play("idle");
        }

        // If the player isn't grounded, default to the falling state
        if (!player.isGrounded)
        {
            player.ySpeed = 0f;
            player.ChangeState(player.fallingState);
        }

        if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            player.ChangeState(player.jetpackState);
        }

        // If movement input detected, enter moving state
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            player.ChangeState(player.movingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }
}
