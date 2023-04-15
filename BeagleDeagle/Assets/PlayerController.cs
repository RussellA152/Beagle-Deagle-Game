using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerState state;

    [SerializeField]
    private TopDownMovement movementScript;

    [SerializeField]
    private PlayerHealth healthScript;

    [SerializeField]
    private GunWeapon weaponEquipped;

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

        // player is currently using their ultimate ability
        Ulting,

        // enemy was killed
        Death

    }
    private void Start()
    {
        state = PlayerState.Idle;

    }

    private void Update()
    {
        if (CheckIfDead())
            state = PlayerState.Death;


        switch (state)
        {
            case PlayerState.Idle:
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                if (CheckIfMoving())
                    state = PlayerState.Moving;
                break;
            case PlayerState.Moving:
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                break;
            case PlayerState.Rolling:
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfAttacking())
                    state = PlayerState.Attacking;
                if (CheckIfMoving())
                    state = PlayerState.Moving;
                break;
            case PlayerState.Attacking:
                if (CheckIfIdle())
                    state = PlayerState.Idle;
                if (CheckIfMoving() && !CheckIfAttacking())
                    state = PlayerState.Moving;
                break;
            case PlayerState.Death:
                Destroy(this.gameObject);
                break;
        }
    }

    private bool CheckIfIdle()
    {
        // if the player is not moving and not attacking
        return (!CheckIfMoving() && !CheckIfAttacking());
    }

    private bool CheckIfMoving()
    {
        // just checking if the player is pressing any movement keys (or moving the left stick)
        return ((Mathf.Abs(movementScript.movementInput.x) > 0f) || (Mathf.Abs(movementScript.movementInput.y) > 0f));
    }

    private bool CheckIfAttacking()
    {
        // checking if the player is attacking with their weapon
        return weaponEquipped.actuallyShooting;
    }

    private bool CheckIfDead()
    {
        // check if the player was killed
        return healthScript.IsDead();
    }

}

