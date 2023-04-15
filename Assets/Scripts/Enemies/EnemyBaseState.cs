using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    protected float timeInState;
    protected float turnToAimTime = 0.15f;
    public abstract void EnterState(EnemyStateManager enemy);
    public virtual void UpdateState(EnemyStateManager enemy)
    {
        timeInState += Time.deltaTime;
    }
    public abstract void ExitState(EnemyStateManager enemy);
}
