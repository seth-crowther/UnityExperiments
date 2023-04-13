using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;
    public EnemyMovingState enemyMovingState;
    public EnemyCoverState enemyCoverState;

    public LayerMask obstacles;
    public Transform player;
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        enemyMovingState = new EnemyMovingState();
        enemyCoverState = new EnemyCoverState();

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
}
