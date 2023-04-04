using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerState state;

    [SerializeField]
    private TopDownMovement movementScript;

    // states that an enemy can be in
    enum PlayerState
    {
        // not moving
        Idle,

        // walking around
        Moving,

        // player used "roll" ability
        Rolling,

        // player is shooting or using their weapon in general
        Attacking,


        // enemy was killed
        Death

    }
    private void Start()
    {
        state = PlayerState.Idle;

    }

    private void Update()
    {

        switch (state)
        {
            case PlayerState.Idle:
                if (CheckIfMoving())
                    state = PlayerState.Moving;
                break;
            case PlayerState.Moving:

                break;
            case PlayerState.Attacking:

                break;
            case PlayerState.Death:

                break;
        }
    }

    private bool CheckIfMoving()
    {
        return (movementScript.movementInput.x > 0f) || (movementScript.movementInput.y > 0f);
    }

}

