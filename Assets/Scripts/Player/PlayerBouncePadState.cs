using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBouncePadState : PlayerBaseState
{
    private static GameObject bouncePad;
    private float pointBeneathPadDistance = 5.0f;
    private Vector3 launchDir;
    private float launchSpeed = 50.0f;

    public static void SetBouncePad(GameObject value)
    {
        bouncePad = value;
    }

    public override void EnterState(PlayerStateManager player)
    {
        launchDir = CalculateLaunchDir(player);
        player.GetRigidbody().AddForce(launchDir * launchSpeed);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);

        // Adjusts y position based on gravity
        player.SetYSpeed(player.GetYSpeed() + PlayerFallingState.gravity * Time.deltaTime);
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

    public Vector3 CalculateLaunchDir(PlayerStateManager player)
    {
        Vector3 pointBeneathPad = bouncePad.transform.position - (bouncePad.transform.up * pointBeneathPadDistance);
        return (player.transform.position - pointBeneathPad).normalized;
    }
}
