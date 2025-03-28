using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SideDetectorScript : MonoBehaviour
{
    DiceRollScript diceRollScript;
    PlayerMovementScript playerMovementScript;
    public GameObject playerFolder;
    private int ind = 0;
    private bool hasMoved = false;

    private float moveCooldown = 1f; // 1 second cooldown
    private float lastMoveTime = 0f;
    void Awake()
    {
        diceRollScript = FindObjectOfType<DiceRollScript>();
        //playerMovementScript = playerPref.GetComponent<PlayerMovementScript>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (diceRollScript != null)
        {
            if (Time.time - lastMoveTime >= moveCooldown)
            {
                if (diceRollScript.GetComponent<Rigidbody>().velocity == Vector3.zero && !hasMoved)
                {
                    diceRollScript.isLanded = true;
                    diceRollScript.diceFaceNum = other.name;

                    PlayerMovementScript[] players = playerFolder.GetComponentsInChildren<PlayerMovementScript>();

                    playerMovementScript = players[ind];
                    playerMovementScript.MovePlayer(Int32.Parse(other.name));
                    hasMoved = true;
                    lastMoveTime = Time.time;

                    if (ind < players.Length - 1)
                    {
                        ind++;
                    }
                    else
                    {
                        ind = 0;
                    }
                }
                else if (diceRollScript.GetComponent<Rigidbody>().velocity != Vector3.zero)
                {
                    diceRollScript.isLanded = false;
                    hasMoved = false;
                }
            }
        }
        else
            Debug.LogError("DiceRollScript not found in a scene!");
    }
}
