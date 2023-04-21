using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingState : EnemyBaseState
{
    public int maxAmmo = 10;
    public int ammo;
    public float reloadTime = 5f;

    private float timeSinceLastShot;
    private float timeBetweenShots = 0.15f;
    private RaycastHit hit;
    private float bulletOvershoot = 5f;

    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.GetAnimator().SetBool("isShooting", true);
        timeInState = 0f;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        base.UpdateState(enemy);
        timeSinceLastShot += Time.deltaTime;

        if (ammo <= 0)
        {
            enemy.ChangeState(EnemyStateManager.EnemyState.coverState);
        }

        // Point towards player
        enemy.transform.forward = (enemy.GetPlayerTransform().position - enemy.transform.position).normalized;

        // Determine line of sight by whether enemy's gun origin can "see" player's gun origin
        Vector3 shootDir = (enemy.GetPlayerGunOrigin().position - enemy.GetGunOrigin().position).normalized;
        Ray lineOfSight = new Ray(enemy.GetGunOrigin().position, shootDir);
        if (Physics.Raycast(lineOfSight, out hit, 200f, enemy.GetPlayerLayerMask()))
        {
            ShootGun(enemy, hit.point + (shootDir * bulletOvershoot));
        }
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.GetAnimator().SetBool("isShooting", false);
    }

    public void ShootGun(EnemyStateManager enemy, Vector3 target)
    {
        if (ammo > 0 && timeSinceLastShot > timeBetweenShots)
        {
            timeSinceLastShot = 0f;
            enemy.HitscanShoot(target);
            ammo--;
        }
        else if (ammo <= 0)
        {
            enemy.ChangeState(EnemyStateManager.EnemyState.coverState);
        }
    }
}
