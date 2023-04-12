using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float finalDist = 5f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navMeshAgent.destination = GetDestination();
    }

    public Vector3 GetDestination()
    {
        if (Vector3.Distance(player.position, transform.position) <= finalDist)
        {
            animator.SetBool("isMoving", false);
            return transform.position;
        }
        
        animator.SetBool("isMoving", true);
        return player.position;
    }
}
