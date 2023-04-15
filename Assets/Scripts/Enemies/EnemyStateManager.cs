using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;
    public EnemyMovingState enemyMovingState;
    public EnemyCoverState enemyCoverState;
    public EnemyShootingState enemyShootingState;

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

    void Start()
    {
        health = maxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();

        enemyMovingState = new EnemyMovingState();
        enemyCoverState = new EnemyCoverState();
        enemyShootingState = new EnemyShootingState();

        currentState = enemyMovingState;
        currentState.EnterState(this);
    }

    void Update()
    {
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
}
