using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Basic Movement and Attack for an AI with one attack and only following.
/// Override OnAttack() and OnChase() functions to make more complex attacks and movement.
/// </summary>
public class AIController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private AIAttack attackScript;

    [SerializeField]
    private EnemyState state;

    [SerializeField]
    private LayerMask attackLayer; // what kind of layer does this enemy attack? (ex. Player)
    [SerializeField]
    private LayerMask chaseLayer; // what kind of layer does this enemy follow? (ex. Player)

    [SerializeField] private bool inAttackRange; // is the player within this enemy's attack range?
    [SerializeField] private bool inChaseRange; // is the player within this enemy's chase/follow range?

    [Header("Line of Sight Values")]
    [SerializeField] private float attackRange;
    [SerializeField] private float chaseRange;

    private void Awake()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // states that an enemy can be in
    enum EnemyState
    {
        // not moving towards player or anyone
        Idle,

        // enemy is spawning behind barrier or from the ground
        Spawning,

        // moving towards player
        Chase,

        // player is in enemy's attack range
        Attack,

        // enemy was killed
        Death

    }

    private void Start()
    {
        state = EnemyState.Idle;


    }

    private void Update()
    {
        inAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, attackLayer);
        inChaseRange = Physics2D.OverlapCircle(transform.position, chaseRange, chaseLayer);


        CheckState();

        switch (state)
        {
            case EnemyState.Idle:
                //Debug.Log("I am IDLE.");
                OnIdle();
                break;
            case EnemyState.Chase:
                //Debug.Log("I am CHASING.");
                OnChase();
                break;
            case EnemyState.Attack:
                //Debug.Log("I am ATTACKING.");
                OnAttack();
                break;
            case EnemyState.Death:
                //Debug.Log("I am DEAD.");
                break;
        }
    }

    protected virtual void CheckState()
    {
        // if the enemy is not dead or currently spawning...
        if(state != EnemyState.Death || state != EnemyState.Spawning)
        {
            // in chase range, but not in attack range
            if (inChaseRange && !inAttackRange)
                state = EnemyState.Chase;

            // in attack range
            else if (inChaseRange && inAttackRange)
                state = EnemyState.Attack;

            // go idle if not in chase range and not in attack range
            else if (!inChaseRange && !inAttackRange)
                state = EnemyState.Idle;
        }
    }
    protected virtual void OnIdle()
    {
        agent.isStopped = true;
    }
    protected virtual void OnChase()
    {
        agent.isStopped = false;

        if (target != null)
            agent.SetDestination(target.position);
    }

    protected virtual void OnAttack()
    {
        agent.isStopped = true;

        attackScript.Attack(target);
    }

    protected virtual void OnDeath(){

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
