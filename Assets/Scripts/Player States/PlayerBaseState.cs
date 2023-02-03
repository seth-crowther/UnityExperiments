using UnityEngine;

// Abstract player state class that all player states inherit from
public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
}
