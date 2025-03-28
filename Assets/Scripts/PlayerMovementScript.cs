using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private Waypoints waypoints;

    [SerializeField] private float moveSpeed = 0.5f;

    private Transform currentWaypoint = null;
    private Transform targetWaypoint;

    private Dictionary<int, int> specialWaypoints = new Dictionary<int, int>();
    private static Dictionary<Vector3, List<PlayerMovementScript>> tileOccupants = new Dictionary<Vector3, List<PlayerMovementScript>>();

    Animator animator;

    void Start()
    {
        waypoints = FindAnyObjectByType<Waypoints>();
        animator = GetComponent<Animator>();

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        string[] specials = ReadLineFromFile("specialWaypoints");
        foreach (string s in specials)
        {
            string[] array = s.Split(',');
            specialWaypoints.Add(int.Parse(array[0]), int.Parse(array[1]));
        }
    }

    public void MovePlayer(int moves)
    {
        StartCoroutine(MoveToSpace(moves));
    }

    public IEnumerator MoveToSpace(int moves)
    {
        animator.SetBool("isWalking", true);
        targetWaypoint = waypoints.GetWaypointFromMoves(moves, currentWaypoint);
        while (currentWaypoint.position != targetWaypoint.position)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

            while (Vector3.Distance(transform.position, currentWaypoint.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = currentWaypoint.position;
            yield return new WaitForSeconds(0.2f);
        }

        foreach (var item in specialWaypoints) 
        {
            if (transform.position == waypoints.GetWaypointAtIndex(item.Key).position)
            {
                targetWaypoint = waypoints.GetWaypointAtIndex(item.Value);
                while (currentWaypoint.position != targetWaypoint.position)
                {
                    currentWaypoint = waypoints.GetWaypointAtIndex(item.Value);
                    while (Vector3.Distance(transform.position, currentWaypoint.position) > 0.01f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
                        yield return null;
                    }

                    transform.position = currentWaypoint.position;
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        if (transform.position == waypoints.GetWaypointAtIndex(100).position)
        {
            Debug.Log("You have won!");
            // Turpinat kodu ðeit...
        }

        animator.SetBool("isWalking", false);
    }

    private string[] ReadLineFromFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (textAsset != null)
            return textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        else
            Debug.LogError("File not found: " + fileName); return new string[0];

    }
}
