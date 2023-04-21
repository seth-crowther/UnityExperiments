using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public enum EnemyState
    {
        currentState,
        movingState,
        coverState,
        shootingState,
        dyingState
    }

    // Enemy states
    private EnemyBaseState currentState;
    private EnemyMovingState movingState;
    private EnemyCoverState coverState;
    private EnemyShootingState shootingState;
    private EnemyDyingState dyingState;

    private NavMeshAgent navMeshAgent;
    private LayerMask onlyPlayer;
    private LayerMask obstacles;

    [SerializeField] private Transform player;
    [SerializeField] private Transform gunOrigin;
    [SerializeField] private Transform playerGunOrigin;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletTrail;

    private int maxHealth = 100;
    private int health;
    private bool isReloading = false;

    # region Getters and setters
    public EnemyBaseState GetState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.currentState:
                return currentState;
            case EnemyState.movingState:
                return movingState;
            case EnemyState.coverState:
                return coverState;
            case EnemyState.shootingState:
                return shootingState;
            case EnemyState.dyingState:
                return dyingState;
            default:
                throw new System.Exception("Enemy state doesn't exist");
        }
    }

    public EnemyCoverState GetCoverState()
    {
        return coverState;
    }

    public EnemyShootingState GetShootingState()
    {
        return shootingState;
    }

    public LayerMask GetPlayerLayerMask()
    {
        return onlyPlayer;
    }

    public LayerMask GetObstaclesLayerMask()
    {
        return obstacles;
    }

    public Transform GetPlayerTransform()
    {
        return player;
    }

    public Transform GetGunOrigin()
    {
        return gunOrigin;
    }

    public Transform GetPlayerGunOrigin()
    {
        return playerGunOrigin;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return navMeshAgent;
    }
    # endregion

    void Start()
    {
        health = maxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();
        onlyPlayer = LayerMask.GetMask("Player");
        obstacles = LayerMask.GetMask("Obstacles");

        movingState = new EnemyMovingState();
        coverState = new EnemyCoverState();
        shootingState = new EnemyShootingState();
        dyingState = new EnemyDyingState();

        currentState = movingState;
        currentState.EnterState(this);
    }

    void Update()
    {
        if (health <= 0)
        {
            ChangeState(EnemyState.dyingState);
        }
        currentState.UpdateState(this);
    }

    public void ChangeState(EnemyState newState)
    {
        currentState.ExitState(this);
        currentState = GetState(newState);
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
            Task.Delay((int)(shootingState.reloadTime * 1000)).ContinueWith(t => ResetAmmo());
        }
    }

    public void ResetAmmo()
    {
        shootingState.ammo = shootingState.maxAmmo;
        isReloading = false;
    }
}
