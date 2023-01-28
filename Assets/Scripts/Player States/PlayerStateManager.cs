using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] public PlayerBaseState currentState;
    public PlayerMovingState movingState;
    public PlayerClimbingState climbingState;
    public Transform climbing;

    public CharacterController controller;
    public Transform groundCheck;
    public Transform headCheck;
    public Transform mainCam;
    public LayerMask groundMask;

    public float ySpeed;

    // Start is called before the first frame update
    void Start()
    {
        movingState = new PlayerMovingState();
        climbingState = new PlayerClimbingState();

        currentState = movingState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(PlayerBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }
}
