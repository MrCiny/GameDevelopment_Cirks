using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, 0.30f);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1) 
        { 
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex());
        }
    }

    public Transform GetWaypointFromMoves(int moves, Transform currentWaypoint, bool isForward = true)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if (isForward)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + moves);
        }
        else
        {
            int ind = 0;
            if (currentWaypoint.GetSiblingIndex() - moves < 0)
                ind = 0;
            else
                ind = currentWaypoint.GetSiblingIndex() - moves;

            return transform.GetChild(ind);
        }
    }

    public Transform GetWaypointAtIndex(int ind)
    {
        return transform.GetChild(ind);
    }

    public Transform GetPreviousWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if (currentWaypoint.GetSiblingIndex() > 0)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() - 1);
        }
        else
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex());
        }
    }
}
