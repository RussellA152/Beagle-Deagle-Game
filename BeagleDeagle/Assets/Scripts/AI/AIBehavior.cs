using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Responsible for executing code based on enemy states. States include: Idle, Chasing, Attack, Stunned, and Death
/// Override OnAttack() and OnChase() functions to make more complex attacks and movement.
/// </summary>
public abstract class AIBehavior<T> : MonoBehaviour, IPoolable, IEnemyDataUpdatable where T: EnemyData
{
    [SerializeField]
    private int poolKey;

    public T enemyScriptableObject;

    [SerializeField] // TEMPORARY, WILL NEED A DIFFERENT WAY TO REFERENCE THE PLAYER *
    private Transform target; // Who this enemy will chase and attack?

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private AIAttack attackScript;

    [SerializeField]
    private AIHealth healthScript;

    [SerializeField]
    private DamageOverTimeHandler damageOverTimeScript;

    [SerializeField]
    private AIMovement movementScript;

    [SerializeField]
    private EnemyState state;

    private bool inAttackRange; // Is the player within this enemy's attack range?
    private bool inChaseRange; // Is the player within this enemy's chase/follow range?

    public int PoolKey => poolKey;

    private void Awake()
    {
        // Prevents AI from spawning with incorrect rotation with NavMeshPlus (2D navmesh asset)
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // States that an enemy can be in
    enum EnemyState
    {
        // Not moving towards player or anyone
        Idle,

        // Enemy is spawning behind barrier or from the ground
        Spawning,

        // Moving towards player
        Chase,

        // Player is in enemy's attack range
        Attack,

        // Enemy was stunned by player or weapon
        Stunned,

        // Enemy was killed
        Death

    }
    private void Start()
    {
        state = EnemyState.Idle;

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void OnEnable()
    {
        state = EnemyState.Idle;

    }

    private void Update()
    {
        inChaseRange = Physics2D.OverlapCircle(transform.position, enemyScriptableObject.chaseRange, enemyScriptableObject.chaseLayer);

        // Only check for attack range if the player is within chase range
        if (inChaseRange)
            inAttackRange = Physics2D.OverlapCircle(transform.position, enemyScriptableObject.attackRange, enemyScriptableObject.attackLayer);
       

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
            case EnemyState.Stunned:
                OnStun();
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

        if (movementScript.isStunned)
        {
            state = EnemyState.Stunned;
            return;
        }

        // If the enemy is not dead or currently spawning...
        if (state != EnemyState.Death || state != EnemyState.Spawning)
        {
            // In chase range, but not in attack range
            if (inChaseRange && !inAttackRange)
                state = EnemyState.Chase;

            // In attack range
            else if (inChaseRange && inAttackRange)
                state = EnemyState.Attack;

            // Go idle if not in chase range and not in attack range
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

    protected virtual void OnDeath()
    {
        gameObject.SetActive(false);

        RevertAllModifiersOnEnemy();

        Debug.Log("I am DEAD.");
    }

    protected virtual void OnStun()
    {
        agent.velocity = Vector2.zero;
        agent.isStopped = true;
    }

    public virtual void UpdateScriptableObject(EnemyData scriptableObject)
    {
        if (scriptableObject is T)
        {
            enemyScriptableObject = scriptableObject as T;
        }
        else
        {
            Debug.Log("ERROR WHEN UPDATING SCRIPTABLE OBJECT! PREFAB DID NOT UPDATE ITS SCRIPTABLE OBJECT");
        }

    }

    public void RevertAllModifiersOnEnemy()
    {
        // Remove all movement modifiers inside of movement script
        movementScript.RevertAllModifiers();
        
        // Remove all health modifiers inside of health script
        healthScript.RevertAllModifiers();

        // Remove all modifiers that affect attack stats within the attack script
        attackScript.RevertAllModifiers();

        // Remove all damage over time effects that the enemy may contain
        damageOverTimeScript.RevertAllModifiers();
    }


    private void OnDrawGizmosSelected()
    {
        // Draw a red sphere indicating how close enemy can be to attack player
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyScriptableObject.attackRange);

        // Draw a blue sphere indiciating how close enemy can be to follow player
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyScriptableObject.chaseRange);
    }
}
