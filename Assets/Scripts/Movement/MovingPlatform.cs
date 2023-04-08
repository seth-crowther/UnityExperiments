using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private WaypointPath waypointPath; // Path of points the moving platform follows
    [SerializeField] private float speed = 5.0f; // Speed at which the platform moves 

    private int nextWayPointIndex = 0; // Index in the array of points of the next point to move to

    private Transform currentWaypoint; // Current target waypoint
    private Transform nextWaypoint; // Next target waypoint

    private float timeToWaypoint; // Time taken to reach the next waypoint at current speed
    private float elapsedTime; // Elapsed time since reaching the last waypoint

    [SerializeField] private LayerMask playerMask;

    void Start()
    {
        // Initialising platform, target the first point in the path
        TargetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculating the percentage of the journey to the next waypoint that has been travelled
        // Then using this value to Smooth the movement so the platform slows as it reaches a waypoint
        elapsedTime += Time.deltaTime;
        float elapsedPercentage = elapsedTime / timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0f, 1f, elapsedPercentage);

        // Lerp function linearly interpolates movement between two points
        // Quaternion.Lerp if you wanna change rotation later
        transform.position = Vector3.Lerp(currentWaypoint.position, nextWaypoint.position, elapsedPercentage);

        // If the journey to the next waypoint is complete, target the next waypoint
        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    // Simple logic to target the next waypoint and update variables
    private void TargetNextWaypoint()
    {
        currentWaypoint = waypointPath.GetWaypoint(nextWayPointIndex);
        nextWayPointIndex = waypointPath.GetNextWaypointIndex(nextWayPointIndex);
        nextWaypoint = waypointPath.GetWaypoint(nextWayPointIndex);

        elapsedTime = 0;

        // Calculating the time it will take to reach the next waypoint based on distance and speed
        float distanceToWaypoint = Vector3.Distance(currentWaypoint.position, nextWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }

    // When the player enters the moving platform trigger, it becomes a child of the moving platform 
    // This means any changes to the transform of the platform will be copied onto the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    // Player must become unparented upon leaving the platform
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
