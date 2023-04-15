using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState currentState; // Current movement state of thep player
    public PlayerMovingState movingState; // Player state when player is moving along the ground
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
    public float walkingSpeed = 6.0f; // Walking speed of the player
    public float turnSmoothTime = 0.1f; // Time to take the player to turn to face desired direction

    public Vector3 inputDirection;
    public Vector3 moveDir;
    public bool isGrounded; // Boolean that updates to indicate whether or not the player is grounded
    public bool hasHitHead; // Boolean that updates to indicate whether ot not the player has hit their head
    public float turnSmoothVelocity; // Speed at which the player turns to face the camera direction
    public float ySpeed; // Y speed of player

    public Camera mainCam;

    private bool shootingState = false;
    private float timeInShootingState;
    private float shootingStateTime = 2f;
    private float turnToAimTime = 0.15f;

    public JetpackRings jetpackParticles;
    public Animator animator;
    private Quaternion rotationOnShoot;

    public int maxHealth = 100;
    public int health;

    public PlayerBaseState GetCurrentState()
    {
        return currentState;
    }

    public bool GetShootingState()
    {
        return shootingState;
    }

    // Initialising player states and defaulting to the falling state
    void Start()
    {
        health = maxHealth;
        mainCam = Camera.main;

        movingState = new PlayerMovingState();
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
            animator.SetBool("isShooting", shootingState);
        }
    }

    public void EnterShootingState()
    {
        timeInShootingState = 0f;
        rotationOnShoot = transform.rotation;
        shootingState = true;
        animator.SetBool("isShooting", shootingState);
    }

    public void HorizontalMovement()
    {
        // Get horizontal and vertical input. "GetAxisRaw" means no input smoothing.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (!shootingState)
        {
            if (inputDirection != Vector3.zero)
            {
                // Calculating desired angle for character to face forward
                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;

                // Smooths turning angle so the target angle is reached in turnSmoothTime seconds
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                Vector3 movement = walkingSpeed * Time.deltaTime * moveDir.normalized;
                controller.Move(movement);
            }
        }
        else
        {
            float mainCamYRot = mainCam.transform.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(rotationOnShoot, Quaternion.Euler(0, mainCamYRot, 0), timeInShootingState / turnToAimTime);

            if (inputDirection != Vector3.zero)
            {
                moveDir = ((horizontal * mainCam.transform.right) + (vertical * mainCam.transform.forward)).normalized;
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

    public void TakeDamage(int amount)
    {
        health -= amount;
        Mathf.Clamp(health, 0f, maxHealth);
    }
}
