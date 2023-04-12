using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.animator.SetBool("inCover", true);
        enemy.animator.SetBool("coverDir", enemy.currentCoverPoint.GetCoverDir());
    }

    public override void UpdateState(EnemyStateManager enemy)
    {

    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.animator.SetBool("inCover", false);
    }
}
