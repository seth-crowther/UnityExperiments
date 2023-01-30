using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJetpackState : PlayerBaseState
{
    public float jetpackJumpHeight = 15.0f;
    public float gravity = -60f;


    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entering jetpack state");
        player.ySpeed = Mathf.Sqrt(jetpackJumpHeight * -2f * gravity);
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

            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 movement = player.walkingSpeed * Time.deltaTime * moveDir.normalized;
            player.controller.Move(movement);
        }

        if (player.hasHitHead)
        {
            player.ySpeed = -1f;
        }

        player.ySpeed += gravity * Time.deltaTime;

        player.controller.Move(new Vector3(0f, player.ySpeed, 0f) * Time.deltaTime);

        if (player.isGrounded && player.ySpeed < 0)
        {
            player.ChangeState(player.movingState);
        }
    }
}
