using UnityEngine;
using System;

public class PlayerFallingState : PlayerBaseState
{
    public float gravity = -60f;
    private float maximumHoverHeight;
    private float minimumHoverHeight;
    private readonly float hoverDistance = 2f;

    private readonly float maxHoverTime = 2f;
    private float elapsedHoverTime;
    private readonly float hoverYSpeedChangeRange = 0.75f;

    public override void EnterState(PlayerStateManager player)
    {
        elapsedHoverTime = 0f;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) // If there is some direction input
        {
            // Calculating desired angle for character to face forward
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + player.mainCam.eulerAngles.y;

            // Smooths turning angle so the target angle is reached in turnSmoothTime seconds
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref player.turnSmoothVelocity, player.turnSmoothTime);

            // Rotating player towards targetAngle slowly, and moving based on direction vector
            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 movement = player.walkingSpeed * Time.deltaTime * moveDir.normalized;
            player.controller.Move(movement);
        }

        // If jump button is held and max hover time hasn't been exceeded, execute hover logic
        // Allows player to hover slightly by using jetpack
        if (Input.GetButton("Jump") && elapsedHoverTime < maxHoverTime)
        {
            // If hover is just about to be initiated, set the range of the hover
            if (elapsedHoverTime == 0f)
            {
                maximumHoverHeight = player.transform.position.y;
                minimumHoverHeight = maximumHoverHeight - hoverDistance;
            }

            elapsedHoverTime += Time.deltaTime;

            // Hover movement logic 
            player.ySpeed += UnityEngine.Random.Range(-hoverYSpeedChangeRange, hoverYSpeedChangeRange);
            // If y position is about to fall below min height or rise above max height
            // Reset y speed and make sure ySpeed is changed to stay within hover range
            float yPosNextFrame = player.transform.position.y + player.ySpeed * Time.deltaTime;
            if (yPosNextFrame < minimumHoverHeight)
            {
                player.ySpeed = 0f;
                player.ySpeed += UnityEngine.Random.Range(0f, hoverYSpeedChangeRange);
            }
            else if (yPosNextFrame > maximumHoverHeight)
            {
                player.ySpeed = 0f;
                player.ySpeed += UnityEngine.Random.Range(-hoverYSpeedChangeRange, 0f);
            }
        }
        else
        {
            // Adjusts y position based on gravity
            player.ySpeed += gravity * Time.deltaTime;
        }

        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);

        // Once player hits the ground, change player state to moving state
        if (player.isGrounded && player.ySpeed < 0)
        {
            player.ChangeState(player.movingState);
        }
    }
}
