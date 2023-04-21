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
        enemy.GetAnimator().SetBool("inCover", true);
        enemy.GetNavMeshAgent().isStopped = true;
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
            enemy.ChangeState(EnemyStateManager.EnemyState.movingState);
        }

        if (enemy.GetShootingState().ammo == enemy.GetShootingState().maxAmmo)
        {
            enemy.ChangeState(EnemyStateManager.EnemyState.shootingState);
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
