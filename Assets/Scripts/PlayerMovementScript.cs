using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private Waypoints waypoints;

    [SerializeField] private float moveSpeed = 0.5f;

    private Transform currentWaypoint = null;
    private Transform targetWaypoint;

    private int currentIndex = 0;

    private Vector3 velocity = Vector3.zero;
    private Dictionary<int, int> specialWaypoints = new Dictionary<int, int>();

    Animator animator;

    void Start()
    {
        waypoints = FindAnyObjectByType<Waypoints>();
        animator = GetComponent<Animator>();
        currentIndex = 0;

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        string[] specials = ReadLineFromFile("specialWaypoints");
        foreach (string s in specials)
        {
            string[] array = s.Split(',');
            specialWaypoints.Add(int.Parse(array[0]), int.Parse(array[1]));
        }
    }

    public void MovePlayer(int moves, bool attack = false)
    {
        if (currentIndex + moves <= 100 && !attack) {
            StartCoroutine(MoveForwardToSpace(moves));
        }
        else if (currentIndex + moves > 100 && !attack)
        {
            int back = - 100 + moves + currentIndex;
            StartCoroutine(MoveBackwardToSpace(back-1));
        }
        else
        {
            StartCoroutine(MoveBackwardToSpace(moves));
        }
    }

    public IEnumerator MoveForwardToSpace(int moves)
    {
        animator.SetBool("isWalking", true);
        targetWaypoint = waypoints.GetWaypointFromMoves(moves, currentWaypoint);
        currentIndex += moves;
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
                currentIndex = item.Value;
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

        CheckForVictory();
        CheckForAttack();

        animator.SetBool("isWalking", false);
    }

    public IEnumerator MoveBackwardToSpace(int moves)
    {
        animator.SetBool("isWalking", true);
        targetWaypoint = waypoints.GetWaypointFromMoves(moves, currentWaypoint, false);
        currentIndex -= moves;
        while (currentWaypoint.position != targetWaypoint.position)
        {
            currentWaypoint = waypoints.GetPreviousWaypoint(currentWaypoint);
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
                    currentIndex = item.Value;
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
        CheckForVictory();
        CheckForAttack();

        animator.SetBool("isWalking", false);
    }

    private void CheckForVictory() 
    {
        if (transform.position == waypoints.GetWaypointAtIndex(100).position)
        {
            Debug.Log("You have won!");
            // Turpinat kodu ðeit...
        }
    }

    private void CheckForAttack()
    {
        PlayerMovementScript[] players = FindObjectsOfType<PlayerMovementScript>();

        foreach (var otherPlayer in players)
        {
            if (otherPlayer != this && otherPlayer.currentIndex == this.currentIndex)
            {
                StartCoroutine(HandleAttack(otherPlayer));
            }
        }
    }

    private IEnumerator HandleAttack(PlayerMovementScript opponent)
    {
        Debug.Log($"Attack: {gameObject.name} vs {opponent.gameObject.name}");

        // Decide winner (random or based on stats)
        bool thisPlayerWins = Random.value > 0.5f;

        PlayerMovementScript loser = thisPlayerWins ? opponent : this;
        Debug.Log($"{loser.gameObject.name} lost the battle!");

        yield return new WaitForSeconds(1f); // Small delay before moving loser back

        loser.MovePlayer(3, true); // Move loser back 3 spaces
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
