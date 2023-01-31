using UnityEngine;

public class PlayerClimbingState : PlayerBaseState
{
    public Transform transform;

    public float climbingSpeed = 5.0f;

    public float speed = 8.0f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private Ray moveDirRay;
    private LayerMask ladder;
    private Vector3 bottomOfPlayer;
    public bool isMovingTowardLadder;

    public override void EnterState(PlayerStateManager player)
    {
        player.ySpeed = 0f;
        ladder = LayerMask.GetMask("Ladder");
        bottomOfPlayer = player.transform.position - new Vector3(0f, 2f, 0f);
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
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            Vector3 movement = speed * Time.deltaTime * moveDir;
            player.controller.Move(movement);

            moveDirRay = new Ray(bottomOfPlayer, moveDir);
            if (Physics.Raycast(moveDirRay, 5f, ladder, QueryTriggerInteraction.Collide))
            {
                player.controller.Move(climbingSpeed * Time.deltaTime * Vector3.up);
            }
        }
    }
}
