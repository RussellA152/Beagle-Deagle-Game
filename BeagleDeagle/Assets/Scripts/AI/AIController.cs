using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Basic Movement and Attack for an AI with one attack and only following.
/// Override OnAttack() and OnChase() functions to make more complex attacks and movement.
/// </summary>
public class AIController : MonoBehaviour, IPoolable
{
    [SerializeField]
    private int poolKey;

    [SerializeField]
    private EnemyData enemyScriptableObject;

    [SerializeField] // TEMPORARY, WILL NEED A DIFFERENT WAY TO REFERENCE THE PLAYER *
    private Transform target; // who this enemy will chase and attack

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private AIAttack attackScript;

    [SerializeField]
    private AIHealth healthScript;

    [SerializeField]
    private EnemyState state;

    [SerializeField]
    private LayerMask attackLayer; // what kind of layer does this enemy attack? (ex. Player)
    [SerializeField]
    private LayerMask chaseLayer; // what kind of layer does this enemy follow? (ex. Player)


    private bool inAttackRange; // is the player within this enemy's attack range?
    private bool inChaseRange; // is the player within this enemy's chase/follow range?

    public int PoolKey => poolKey;

    private void Awake()
    {
        // prevents AI from spawning with incorrect rotation with NavMeshPlus (2D navmesh asset)
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

        Stunned,

        // enemy was killed
        Death

    }
    private void Start()
    {
        state = EnemyState.Idle;

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void OnEnable()
    {
        agent.speed = enemyScriptableObject.movementSpeed;
    }

    private void Update()
    {
        inAttackRange = Physics2D.OverlapCircle(transform.position, enemyScriptableObject.attackRange, attackLayer);
        inChaseRange = Physics2D.OverlapCircle(transform.position, enemyScriptableObject.chaseRange, chaseLayer);


        CheckState();

        switch (state)
        {
            case EnemyState.Idle:
                OnIdle();
                break;
            case EnemyState.Chase:
                OnChase();
                break;
            case EnemyState.Attack:
                OnAttack();
                break;
            case EnemyState.Death:
                OnDeath();  
                break;
        }
    }

    protected virtual void CheckState()
    {
        if (healthScript.IsDead())
        {
            state = EnemyState.Death;
            return;
        }
            

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
        gameObject.SetActive(false);

        Debug.Log("I am DEAD.");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyScriptableObject.attackRange);

        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyScriptableObject.chaseRange);
    }

}
