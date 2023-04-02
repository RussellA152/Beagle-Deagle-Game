using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private AIAttack attackScript;

    private EnemyState state;

    [SerializeField]
    private LayerMask attackLayer; // what kind of layer does this enemy attack? (ex. Player)
    [SerializeField]
    private LayerMask chaseLayer; // what kind of layer does this enemy follow? (ex. Player)

    private bool inAttackRange; // is the player within this enemy's attack range?
    private bool inChaseRange; // is the player within this enemy's chase/follow range?

    [Header("Line of Sight Values")]
    [SerializeField] private float attackRange;
    [SerializeField] private float chaseRange;

    // states that an enemy can be in
    enum EnemyState
    {
        // not moving towards player or anyone
        idle,

        // enemy is spawning behind barrier or from the ground
        spawning,

        // moving towards player
        chase,

        // player is in enemy's attack range
        attack,

        // enemy was killed
        death

    }

    private void Start()
    {
        state = EnemyState.idle;
    }

    private void Update()
    {
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, attackLayer);
        inChaseRange = Physics.CheckSphere(transform.position, chaseRange, chaseLayer);

        CheckState();

        switch (state)
        {
            case EnemyState.idle:
                Debug.Log("I am IDLE.");
                break;
            case EnemyState.chase:
                Debug.Log("I am CHASING.");
                break;
            case EnemyState.attack:
                Debug.Log("I am ATTACKING.");
                break;
            case EnemyState.death:
                Debug.Log("I am DEAD.");
                break;
        }
    }

    protected virtual void CheckState()
    {
        // if the enemy is not dead or currently spawning...
        if(state != EnemyState.death || state != EnemyState.spawning)
        {
            if (inChaseRange && !inAttackRange)
                state = EnemyState.chase;

            else if (inChaseRange && inAttackRange)
                state = EnemyState.attack;

            else if (!inChaseRange && !inAttackRange)
                state = EnemyState.idle;
        }
    }

    protected virtual void OnAttack()
    {
        attackScript.Attack();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

}
