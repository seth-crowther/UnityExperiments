using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState currentState;

    public CharacterController controller;
    public Transform groundCheck;
    public Transform mainCam;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovingState movingState = new PlayerMovingState(controller, groundCheck, mainCam, transform);
        PlayerClimbingState climbingState = new PlayerClimbingState();

        currentState = movingState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }
}
