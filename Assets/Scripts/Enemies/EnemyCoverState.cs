using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverState : EnemyBaseState
{
    private EnemyCover.CoverPoint coverPoint;

    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.animator.SetBool("inCover", true);
        enemy.transform.forward = coverPoint.transform.forward;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (!coverPoint.IsValid())
        {
            enemy.ChangeState(enemy.enemyMovingState);
        }
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.animator.SetBool("inCover", false);
    }

    public void SetCoverPoint(EnemyCover.CoverPoint coverPoint)
    {
        this.coverPoint = coverPoint;
    }
}
