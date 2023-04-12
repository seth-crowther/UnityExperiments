using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float finalDist;
    private float delta = 0.25f;

    void Start()
    {
        finalDist = 5f;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navMeshAgent.destination = GetDestination();
    }

    public Vector3 GetDestination()
    {
        if (Vector3.Distance(player.position, transform.position) <= finalDist + delta)
        {
            animator.SetBool("isMoving", false);
            return transform.position;
        }

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        animator.SetBool("isMoving", true);
        return player.position - (dirToPlayer * finalDist);
    }
}
