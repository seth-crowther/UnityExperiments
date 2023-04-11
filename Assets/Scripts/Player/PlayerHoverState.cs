using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoverState : PlayerBaseState
{
    private bool hoverComplete = false;

    private float maximumHoverHeight;
    private float minimumHoverHeight;

    private readonly float hoverDistance = 2f;
    private float elapsedHoverTime;
    private float maxHoverTime = 2f;
    private readonly float hoverYSpeedChangeRange = 0.75f;

    public bool GetHoverComplete()
    {
        return hoverComplete;
    }

    public void SetHoverComplete(bool value)
    {
        hoverComplete = value;
    }

    public void SetElapsedHoverTime(float value)
    {
        elapsedHoverTime = value;
    }

    public override void EnterState(PlayerStateManager player)
    {
        // Set the range of the hover
        maximumHoverHeight = player.transform.position.y;
        minimumHoverHeight = maximumHoverHeight - hoverDistance;
        player.jetpackParticles.PlayParticles();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);

        // If you let go of Space, enter falling state
        if (!Input.GetButton("Jump"))
        {
            player.ChangeState(player.fallingState);
        }

        // Hover time ticks up
        elapsedHoverTime += Time.deltaTime;

        // Once maximum hover time has expired, enter falling state and set hoverComplete to true
        if (elapsedHoverTime > maxHoverTime)
        {
            hoverComplete = true;
            player.ChangeState(player.fallingState);
        }

        // Hover movement logic 
        player.ySpeed += Random.Range(-hoverYSpeedChangeRange, hoverYSpeedChangeRange);
        // If y position is about to fall below min height or rise above max height
        // Reset y speed and make sure ySpeed is changed to stay within hover range
        float yPosNextFrame = player.transform.position.y + player.ySpeed * Time.deltaTime;
        if (yPosNextFrame < minimumHoverHeight)
        {
            player.ySpeed = 0f;
            player.ySpeed += Random.Range(0f, hoverYSpeedChangeRange);
        }
        else if (yPosNextFrame > maximumHoverHeight)
        {
            player.ySpeed = 0f;
            player.ySpeed += Random.Range(-hoverYSpeedChangeRange, 0f);
        }

        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.jetpackParticles.StopParticles();
    }
}
