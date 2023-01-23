using UnityEngine;

public class PlayerClimbingState : PlayerBaseState
{
    public Transform transform;
    public Transform climbing;

    public float climbingSpeed = 5.0f;
    public float ySpeed;

    public float speed = 8.0f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    

    public override void EnterState(PlayerStateManager player)
    {

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

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 movement = speed * Time.deltaTime * moveDir.normalized;
            player.controller.Move(movement);
        }

        // Do some logic based on where camera's looking to determine which directional button means go up the ladder
        ySpeed = vertical * climbingSpeed * Time.deltaTime;
        player.controller.Move(new Vector3(0f, ySpeed, 0f));
    }
}
