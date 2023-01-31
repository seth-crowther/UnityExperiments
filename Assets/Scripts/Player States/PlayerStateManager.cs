using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState currentState;
    public PlayerMovingState movingState;
    public PlayerClimbingState climbingState;
    public PlayerJetpackState jetpackState;
    public PlayerFallingState fallingState; 

    public CharacterController controller;
    public Transform groundCheck;
    public Transform headCheck;
    public Transform mainCam;
    public LayerMask groundMask;

    public float groundDistance = 1.0f;
    public float headDistance = 0.1f;
    public float walkingSpeed = 16.0f;
    public float turnSmoothTime = 0.1f;

    public bool isGrounded;
    public bool hasHitHead;
    public float turnSmoothVelocity;

    public float ySpeed;

    // Start is called before the first frame update
    void Start()
    {
        movingState = new PlayerMovingState();
        climbingState = new PlayerClimbingState();
        jetpackState = new PlayerJetpackState();
        fallingState = new PlayerFallingState();

        currentState = fallingState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);
        if (!isGrounded)
        {
            hasHitHead = Physics.CheckSphere(headCheck.position, headDistance, groundMask, QueryTriggerInteraction.Ignore);
        }

        currentState.UpdateState(this);
    }

    public void ChangeState(PlayerBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }
}
