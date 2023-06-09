using UnityEngine;
using System;

public class PlayerFallingState : PlayerBaseState
{
    public static float gravity = -60f;
    
    public override void EnterState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);

        // Adjusts y position based on gravity
        player.SetYSpeed(player.GetYSpeed() + gravity * Time.deltaTime);

        player.GetController().Move(new Vector3(0f, player.GetYSpeed(), 0f) * Time.deltaTime);

        // Once player hits the ground, change player state to moving state
        if (player.isGrounded && player.GetYSpeed() < 0)
        {
            player.ChangeState(PlayerStateManager.PlayerState.movingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }
}
