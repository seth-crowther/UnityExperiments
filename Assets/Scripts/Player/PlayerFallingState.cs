using UnityEngine;
using System;

public class PlayerFallingState : PlayerBaseState
{
    public float gravity = -60f;
    
    public override void EnterState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);

        // If jump button is held and a hover hasn't been completed, change to hover state
        // Allows player to hover slightly for a short time by using jetpack
        if (Input.GetButton("Jump") && !player.GetHoverState().GetHoverComplete())
        {
            player.ChangeState(PlayerStateManager.PlayerState.hoverState);
        }
        else
        {
            // Adjusts y position based on gravity
            player.ySpeed += gravity * Time.deltaTime;
        }

        player.GetController().Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);

        // Once player hits the ground, change player state to moving state
        if (player.isGrounded && player.ySpeed < 0)
        {
            player.ChangeState(PlayerStateManager.PlayerState.movingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }
}
