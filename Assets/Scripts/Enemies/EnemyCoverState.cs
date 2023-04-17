using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyCoverState : EnemyBaseState
{
    private EnemyCover.CoverPoint coverPoint;
    private Quaternion rotationOnEnter;

    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.animator.SetBool("inCover", true);
        enemy.navMeshAgent.isStopped = true;
        timeInState = 0f;
        rotationOnEnter = enemy.transform.rotation;

        enemy.Reload();
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        base.UpdateState(enemy);

        // Turning enemy towards correct direction
        enemy.transform.rotation = Quaternion.Slerp(rotationOnEnter, coverPoint.transform.rotation, timeInState / turnToAimTime);

        if (!coverPoint.IsValid())
        {
            enemy.ChangeState(enemy.enemyMovingState);
        }

        if (enemy.enemyShootingState.ammo == enemy.enemyShootingState.maxAmmo)
        {
            enemy.ChangeState(enemy.enemyShootingState);
        }
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        
    }

    public void SetCoverPoint(EnemyCover.CoverPoint coverPoint)
    {
        this.coverPoint = coverPoint;
    }
}
