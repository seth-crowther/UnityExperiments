using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum PlayerState
    {
        currentState,
        movingState,
        jetpackState,
        fallingState,
        hoverState,
        idleState
    }

    // Player states
    private PlayerBaseState currentState; // Current movement state of the player
    private PlayerMovingState movingState; // Player state when player is moving along the ground
    private PlayerJetpackState jetpackState; // Player state when player is jumping with the jetpack
    private PlayerFallingState fallingState; // Player state when player is falling under gravity
    private PlayerHoverState hoverState; // Player state when player is hovering with the jetpack
    private PlayerIdleState idleState;

    private Camera mainCam;

    [SerializeField] private CharacterController controller; // Reference to the player's character controller component
    [SerializeField] private Transform groundCheck; // Empty object that is used to check if player is grounded
    [SerializeField] private Transform headCheck; // Empty object that is used to check if player has hit their head
    [SerializeField] private LayerMask groundMask; // Layer mask that includes all objects that can be walked on
    [SerializeField] private Animator animator;
    [SerializeField] private JetpackRings jetpackParticles;
    [SerializeField] private HandleGun gun;

    private float groundDistance = 1.0f; // Radius of sphere of grounded check using groundCheck
    private float headDistance = 0.1f; // Radius of sphere of hitting head check using headCheck
    private float walkingSpeed = 6.0f; // Walking speed of the player
    private float turnSmoothTime = 0.1f; // Time to take the player to turn to face desired direction

    private Vector3 inputDirection;
    private bool shootingState = false;
    private float timeInShootingState;
    private float shootingStateTime = 2f;
    private float turnToAimTime = 0.15f;
    private Quaternion rotationOnShoot;
    private int maxHealth = 100;
    private int health;
    private Vector3 moveDir;

    public bool isGrounded; // Boolean that updates to indicate whether or not the player is grounded
    public bool hasHitHead; // Boolean that updates to indicate whether ot not the player has hit their head
    public float turnSmoothVelocity; // Speed at which the player turns to face the camera direction
    public float ySpeed; // Y speed of player

    # region Getters and Setters 
    private PlayerBaseState GetState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.currentState:
                return currentState;
            case PlayerState.movingState:
                return movingState;
            case PlayerState.jetpackState:
                return jetpackState;
            case PlayerState.fallingState:
                return fallingState;
            case PlayerState.hoverState:
                return hoverState;
            case PlayerState.idleState:
                return idleState;

            default:
                throw new System.Exception("State doesn't exist");
        }
    }

    public PlayerBaseState GetCurrentState()
    {
        return currentState;
    }

    public PlayerMovingState GetMovingState()
    {
        return movingState;
    }

    public PlayerJetpackState GetJetpackState()
    {
        return jetpackState;
    }

    public PlayerFallingState GetFallingState()
    {
        return fallingState;
    }

    public PlayerHoverState GetHoverState()
    {
        return hoverState;
    }

    public PlayerIdleState GetIdleState()
    {
        return idleState;
    }

    public CharacterController GetController()
    {
        return controller;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public void SetShootingState(bool value)
    {
        shootingState = value;
    }

    public Vector3 GetInputDirection()
    {
        return inputDirection;
    }

    public bool GetShootingState()
    {
        return shootingState;
    }

    public int GetHealth()
    {
        return health;
    }

    # endregion

    // Initialising player states and defaulting to the falling state
    void Start()
    {
        health = maxHealth;

        mainCam = Camera.main;

        movingState = new PlayerMovingState();
        jetpackState = new PlayerJetpackState();
        fallingState = new PlayerFallingState();
        hoverState = new PlayerHoverState(jetpackParticles);
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
        if (!gun.IsReloading())
        {
            timeInShootingState = 0f;
            rotationOnShoot = transform.rotation;
            shootingState = true;
            animator.SetBool("isShooting", shootingState);
        }
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
    public void ChangeState(PlayerState newState)
    {
        Debug.Log("Switching to " + newState);
        currentState.ExitState(this);
        currentState = GetState(newState);
        currentState.EnterState(this);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Mathf.Clamp(health, 0f, maxHealth);
    }

    
}
