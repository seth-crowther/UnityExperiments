using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState : EnemyBaseState
{
    private Collider[] nearbyObstacles;
    private float coverSightRange = 15f;
    private float minPlayerDist = 5f;
    private float delta = 0.25f;

    public override void EnterState(EnemyStateManager enemy)
    {
        
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        enemy.navMeshAgent.destination = GetDestination(enemy);
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        
    }

    public Vector3 GetDestination(EnemyStateManager enemy)
    {
        List<GameObject> nearbyCover = new List<GameObject>();
        nearbyObstacles = Physics.OverlapSphere(enemy.transform.position, coverSightRange, enemy.obstacles);

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
            
            // If there aren't any valid cover points, go towards player
            if (validCoverPoints.Count == 0)
            {
                return enemy.player.position;
            }

            EnemyCover.CoverPoint destination = GetClosestCoverPoint(validCoverPoints, enemy);

            if (HasArrivedAt(destination.GetPoint(), enemy))
            {
                enemy.enemyCoverState.SetCoverPoint(destination);
                enemy.ChangeState(enemy.enemyCoverState);
            }
            else
            {
                enemy.animator.SetBool("isMoving", true);
            }

            return destination.GetPoint();
        }

        if (Vector3.Distance(enemy.player.position, enemy.transform.position) <= minPlayerDist)
        {
            enemy.animator.SetBool("isMoving", false);
            return enemy.transform.position;
        }

        enemy.animator.SetBool("isMoving", true);
        return enemy.player.position;
    }

    public EnemyCover.CoverPoint GetClosestCoverPoint(HashSet<EnemyCover.CoverPoint> coverPoints, EnemyStateManager enemy)
    {
        float minDist = 9999f;
        EnemyCover.CoverPoint toReturn = null;
        foreach (EnemyCover.CoverPoint cp in coverPoints)
        {
            if (Vector3.Distance(cp.GetPoint(), enemy.transform.position) < minDist)
            {
                toReturn = cp;
                minDist = Vector3.Distance(cp.GetPoint(), enemy.transform.position);
            }
        }

        return toReturn;
    }

    public bool HasArrivedAt(Vector3 position, EnemyStateManager enemy)
    {
        return Vector3.Distance(position, enemy.transform.position) < delta;
    }
}
