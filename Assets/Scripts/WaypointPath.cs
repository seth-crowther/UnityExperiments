using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Empty object with child empty objects that represent the path a moving platform should follow
public class WaypointPath : MonoBehaviour
{
    // Gets the waypoint at a given index
    public Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    // Gets the next waypoint in the path
    // Using % so the path repeats once completed
    public int GetNextWaypointIndex(int currentIndex)
    {
        return (currentIndex + 1) % transform.childCount;
    }
}
