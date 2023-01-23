using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private WaypointPath waypointPath;
    [SerializeField] private float speed = 5.0f;

    private int nextWayPointIndex = 0;

    private Transform currentWaypoint;
    private Transform nextWaypoint;

    private float timeToWaypoint;
    private float elapsedTime;

    [SerializeField] private LayerMask playerMask;

    // Start is called before the first frame update
    void Start()
    {
        TargetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0f, 1f, elapsedPercentage);
        transform.position = Vector3.Lerp(currentWaypoint.position, nextWaypoint.position, elapsedPercentage);
        // Quaternion.Lerp if you wanna change rotation later

        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        currentWaypoint = waypointPath.GetWaypoint(nextWayPointIndex);
        nextWayPointIndex = waypointPath.GetNextWaypointIndex(nextWayPointIndex);
        nextWaypoint = waypointPath.GetWaypoint(nextWayPointIndex);

        elapsedTime = 0;
        float distanceToWaypoint = Vector3.Distance(currentWaypoint.position, nextWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
