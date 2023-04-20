using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJetpackState : PlayerBaseState
{
    public float jetpackJumpHeight = 15.0f;
    public float gravity = -60f;


    public override void EnterState(PlayerStateManager player)
    {
        player.ySpeed = Mathf.Sqrt(jetpackJumpHeight * -2f * gravity);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);

        // If player hits head on an object while jumping, y velocity is reset
        if (player.hasHitHead)
        {
            player.ySpeed = -1f;
        }

        if (player.ySpeed < 0)
        {
            // Once player hits the ground, change player state to moving state
            if (player.isGrounded)
            {
                player.ChangeState(PlayerStateManager.PlayerState.movingState);
            }
            else
            {
                player.ChangeState(PlayerStateManager.PlayerState.fallingState);
            }
        }

        // Adjusts y position based on gravity
        player.ySpeed += gravity * Time.deltaTime;
        player.GetController().Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }
}
