using UnityEngine;

public class PlayerClimbingState : PlayerBaseState
{
    public Transform transform;

    public float climbingSpeed = 5.0f;

    public float speed = 8.0f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private Ray moveDirRay;
    private LayerMask obstacles;
    private Vector3 bottomOfPlayer;
    public bool isMovingTowardLadder;

    public override void EnterState(PlayerStateManager player)
    {
        player.ySpeed = 0f;
        obstacles = LayerMask.GetMask("Obstacles");
        bottomOfPlayer = player.transform.position - new Vector3(0f, 2f, 0f);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        base.UpdateState(player);
        

        // TODO: Implement climbing logic
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }
}
