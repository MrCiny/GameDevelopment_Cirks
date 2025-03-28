using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float waypointSize = 0.30f;
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

    public Transform GetWaypointFromMoves(int moves, Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        int left_spaces = 200 - moves - currentWaypoint.GetSiblingIndex();

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1 - moves)
        {;
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + moves);
        }
        else
        {
            Debug.Log(left_spaces);
            return transform.GetChild(left_spaces);
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

        return null;
    }
}
