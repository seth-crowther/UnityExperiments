using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDyingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.GetAnimator().SetBool("isDead", true);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        base.UpdateState(enemy);
        if (timeInState >= enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).length)
        {
            enemy.Kill();
        }
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        
    }
}
