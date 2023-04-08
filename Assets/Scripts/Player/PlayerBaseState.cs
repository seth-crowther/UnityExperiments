using UnityEngine;

// Abstract player state class that all player states inherit from
public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public virtual void UpdateState(PlayerStateManager player)
    {
        player.HorizontalMovement();
    }
}
