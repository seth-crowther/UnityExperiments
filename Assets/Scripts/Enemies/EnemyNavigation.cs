using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float minPlayerDist = 5f;
    private Collider[] nearbyObstacles;
    private float coverSightRange = 15f;
    [SerializeField] private LayerMask obstacles;
    private float delta = 2.1f;

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
        List<GameObject> nearbyCover = new List<GameObject>();
        nearbyObstacles = Physics.OverlapSphere(transform.position, coverSightRange, obstacles);

        foreach (Collider c in nearbyObstacles)
        {
            if (c.gameObject.CompareTag("Cover"))
            {
                nearbyCover.Add(c.gameObject);
            }
        }

        if (nearbyCover.Count > 0)
        {
            HashSet<EnemyCover.CoverPoint> validCoverPoints = new HashSet<EnemyCover.CoverPoint>();
            foreach (GameObject g in nearbyCover)
            {
                foreach (EnemyCover.CoverPoint cp in g.GetComponent<EnemyCover>().GetValidCoverPoints())
                {
                    validCoverPoints.Add(cp);
                }
            }

            Vector3 destination = GetClosestCoverPoint(validCoverPoints).GetPoint();

            if (Vector3.Distance(transform.position, destination) < delta)
            {
                animator.SetBool("isMoving", false);
            }
            else
            {
                animator.SetBool("isMoving", true);
            }

            return destination;
        }

        if (Vector3.Distance(player.position, transform.position) <= minPlayerDist)
        {
            animator.SetBool("isMoving", false);
            return transform.position;
        }

        animator.SetBool("isMoving", true);
        return player.position;
    }

    public EnemyCover.CoverPoint GetClosestCoverPoint(HashSet<EnemyCover.CoverPoint> coverPoints)
    {
        float minDist = 9999f;
        EnemyCover.CoverPoint toReturn = null;
        foreach (EnemyCover.CoverPoint cp in coverPoints)
        {
            if (Vector3.Distance(cp.GetPoint(), transform.position) < minDist)
            {
                toReturn = cp;
                minDist = Vector3.Distance(cp.GetPoint(), transform.position);
            }
        }
        return toReturn;
    }
}
