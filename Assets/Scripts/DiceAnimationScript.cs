using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAnimationScript : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RollDice()
    {
        animator.SetBool("isRolling", true);
    }

    public void StopDiceRoll()
    {
        animator.SetBool("isRolling", false);
    }
}
