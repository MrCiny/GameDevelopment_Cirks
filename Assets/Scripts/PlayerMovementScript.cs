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

    private CameraScript mainCamera;
    private Dictionary<int, int> specialWaypoints = new Dictionary<int, int>();
    private Dictionary<int, List<PlayerMovementScript>> tileOccupants = new Dictionary<int, List<PlayerMovementScript>>();


    Animator animator;

    void Start()
    {
        waypoints = FindAnyObjectByType<Waypoints>();
        animator = GetComponent<Animator>();
        mainCamera = FindAnyObjectByType<CameraScript>();
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
            int targetIndex = 200 - currentIndex - moves;
            if (targetIndex > currentIndex)
                StartCoroutine(MoveForwardToSpace(targetIndex - currentIndex));
            else
                StartCoroutine(MoveBackwardToSpace(currentIndex - targetIndex));
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
                mainCamera.UpdateCamera(transform);
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
                        mainCamera.UpdateCamera(transform);
                        yield return null;
                    }

                    transform.position = currentWaypoint.position;
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        mainCamera.ResetCamera();
        CheckForVictory();
        CheckForAttack();

        animator.SetBool("isWalking", false);
    }

    public IEnumerator MoveBackwardToSpace(int moves)
    {
        animator.SetBool("isWalking", true);
        targetWaypoint = waypoints.GetWaypointFromMoves(moves, currentWaypoint, false);
        if (currentIndex - moves < 0)
            currentIndex = 0;
        else
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
            mainCamera.UpdateCamera(transform);
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
                        mainCamera.UpdateCamera(transform);
                        yield return null;
                    }

                    transform.position = currentWaypoint.position;
                    mainCamera.UpdateCamera(transform);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        mainCamera.ResetCamera();
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
        Vector3 curVel = Vector3.zero;
        Vector3 opTargetPos = opponent.transform.position + new Vector3(0.2f, 0, 0);
        Vector3 targetPos = transform.position - new Vector3(0.2f, 0, 0);
        opponent.transform.position = opTargetPos;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position,targetPos, ref curVel, moveSpeed * Time.deltaTime);
            opponent.transform.position = Vector3.MoveTowards(opponent.transform.position, opTargetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);

        Debug.Log($"Attack: {gameObject.name} vs {opponent.gameObject.name}");

        bool thisPlayerWins = Random.value > 0.5f;

        PlayerMovementScript loser = thisPlayerWins ? opponent : this;

        // Japieliek animacija
        Debug.Log($"{loser.gameObject.name} lost the battle!");

        yield return new WaitForSeconds(3f);

        loser.MovePlayer(3, true);
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
