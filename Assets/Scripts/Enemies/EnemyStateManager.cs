using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;
    public EnemyMovingState enemyMovingState;
    public EnemyCoverState enemyCoverState;
    public EnemyShootingState enemyShootingState;
    public EnemyDyingState enemyDyingState;

    public LayerMask onlyPlayer;
    public LayerMask obstacles;
    public Transform player;
    public Transform gunOrigin;
    public Transform playerGunOrigin;
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    public GameObject bulletTrail;

    public int maxHealth = 100;
    public int health;
    private bool isReloading = false;

    void Start()
    {
        health = maxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();

        enemyMovingState = new EnemyMovingState();
        enemyCoverState = new EnemyCoverState();
        enemyShootingState = new EnemyShootingState();
        enemyDyingState = new EnemyDyingState();

        currentState = enemyMovingState;
        currentState.EnterState(this);
    }

    void Update()
    {
        if (health <= 0)
        {
            ChangeState(enemyDyingState);
        }
        currentState.UpdateState(this);
    }

    public void ChangeState(EnemyBaseState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void HitscanShoot(Vector3 target)
    {
        GameObject bullet = Instantiate(bulletTrail, gunOrigin.position, Quaternion.identity);
        bullet.GetComponent<HitscanShoot>().SetTarget(target);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Mathf.Clamp(health, 0, maxHealth);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void Reload()
    {
        if (!isReloading)
        {
            isReloading = true;
            Task.Delay((int)(enemyShootingState.reloadTime * 1000)).ContinueWith(t => ResetAmmo());
        }
    }

    public void ResetAmmo()
    {
        enemyShootingState.ammo = enemyShootingState.maxAmmo;
        isReloading = false;
    }
}
