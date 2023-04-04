using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState currentState; // Current movement state of thep player
    public PlayerMovingState movingState; // Player state when player is moving along the ground
    public PlayerClimbingState climbingState; // Player state when player is climbing a climable object
    public PlayerJetpackState jetpackState; // Player state when player is jumping with the jetpack
    public PlayerFallingState fallingState; // Player state when player is falling under gravity
    public PlayerHoverState hoverState;

    public CharacterController controller; // Reference to the player's character controller component
    public Transform groundCheck; // Empty object that is used to check if player is grounded
    public Transform headCheck; // Empty object that is used to check if player has hit their head
    public Transform mainCam; // Main camera reference
    public LayerMask groundMask; // Layer mask that includes all objects that can be walked on

    public float groundDistance = 1.0f; // Radius of sphere of grounded check using groundCheck
    public float headDistance = 0.1f; // Radius of sphere of hitting head check using headCheck
    public float walkingSpeed = 16.0f; // Walking speed of the player
    public float turnSmoothTime = 0.1f; // Time to take the player to turn to face the camera's direction

    public bool isGrounded; // Boolean that updates to indicate whether or not the player is grounded
    public bool hasHitHead; // Boolean that updates to indicate whether ot not the player has hit their head
    public float turnSmoothVelocity; // Speed at which the player turns to face the camera direction

    public float ySpeed; // Y speed of player

    // Initialising player states and defaulting to the falling state
    void Start()
    {
        movingState = new PlayerMovingState();
        climbingState = new PlayerClimbingState();
        jetpackState = new PlayerJetpackState();
        fallingState = new PlayerFallingState();
        hoverState = new PlayerHoverState();

        currentState = fallingState;
        currentState.EnterState(this);
    }

    
    void Update()
    {
        // Updates isGrounded and hasHitHead booleans
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);
        if (!isGrounded)
        {
            hasHitHead = Physics.CheckSphere(headCheck.position, headDistance, groundMask, QueryTriggerInteraction.Ignore);
        }

        // Updates player based on the current player state's update method
        currentState.UpdateState(this);
    }

    public void HorizontalMovement()
    {
        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) // If there is some direction input
        {
            // Calculating desired angle for character to face forward
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;

            // Smooths turning angle so the target angle is reached in turnSmoothTime seconds
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Rotating player towards targetAngle slowly, and moving based on direction vector
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 movement = walkingSpeed * Time.deltaTime * moveDir.normalized;
            controller.Move(movement);
        }
    }

    // Simple function to switch states
    public void ChangeState(PlayerBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    public void TurnWithCamera()
    {

    }
}
