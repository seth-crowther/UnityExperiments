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
    public PlayerHoverState hoverState; // Player state when player is hovering with the jetpack
    public PlayerIdleState idleState;

    public CharacterController controller; // Reference to the player's character controller component
    public Transform groundCheck; // Empty object that is used to check if player is grounded
    public Transform headCheck; // Empty object that is used to check if player has hit their head
    public LayerMask groundMask; // Layer mask that includes all objects that can be walked on

    public float groundDistance = 1.0f; // Radius of sphere of grounded check using groundCheck
    public float headDistance = 0.1f; // Radius of sphere of hitting head check using headCheck
    public float walkingSpeed = 16.0f; // Walking speed of the player
    public float turnSmoothTime = 0.1f; // Time to take the player to turn to face desired direction

    public bool isGrounded; // Boolean that updates to indicate whether or not the player is grounded
    public bool hasHitHead; // Boolean that updates to indicate whether ot not the player has hit their head
    public float turnSmoothVelocity; // Speed at which the player turns to face the camera direction
    public float ySpeed; // Y speed of player

    public Camera mainCam;

    private bool shootingState = false;
    private float timeInShootingState;
    private float shootingStateTime = 2f;
    private float turnToAimSpeed = 1.0f;

    public JetpackRings jetpackParticles;
    public Animator animator;

    public PlayerBaseState GetCurrentState()
    {
        return currentState;
    }

    // Initialising player states and defaulting to the falling state
    void Start()
    {
        mainCam = Camera.main;

        movingState = new PlayerMovingState();
        climbingState = new PlayerClimbingState();
        jetpackState = new PlayerJetpackState();
        fallingState = new PlayerFallingState();
        hoverState = new PlayerHoverState();
        idleState = new PlayerIdleState();

        currentState = idleState;
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

        if (shootingState)
        {
            timeInShootingState += Time.deltaTime;
        }

        if (timeInShootingState > shootingStateTime)
        {
            shootingState = false;
        }
    }

    public void EnterShootingState()
    {
        timeInShootingState = 0f;
        shootingState = true;
    }

    public void HorizontalMovement()
    {
        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (!shootingState)
        {
            if (direction.magnitude >= 0.1f)
            {
                // Calculating desired angle for character to face forward
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;

                // Smooths turning angle so the target angle is reached in turnSmoothTime seconds
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                Vector3 movement = walkingSpeed * Time.deltaTime * moveDir.normalized;
                controller.Move(movement);
            }
        }
        else
        {
            float mainCamYRot = mainCam.transform.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamYRot, 0), timeInShootingState / turnToAimSpeed);

            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = ((horizontal * mainCam.transform.right) + (vertical * mainCam.transform.forward)).normalized;
                controller.Move(moveDir * walkingSpeed * Time.deltaTime);
            }
        }
    }

    // Simple function to switch states
    public void ChangeState(PlayerBaseState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}
