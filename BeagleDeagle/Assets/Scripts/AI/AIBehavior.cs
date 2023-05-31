using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Responsible for executing code based on enemy states. States include: Idle, Chasing, Attack, Stunned, and Death
/// Override OnAttack() and OnChase() functions to make more complex attacks and movement.
/// </summary>
public abstract class AIBehavior<T> : MonoBehaviour, IPoolable, IMovable, IEnemyDataUpdatable where T: EnemyData
{
    [SerializeField]
    private int poolKey;

    public T enemyScriptableObject;

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

    private bool inAttackRange; // is the player within this enemy's attack range?
    private bool inChaseRange; // is the player within this enemy's chase/follow range?

    public int PoolKey => poolKey;

    [SerializeField, NonReorderable]
    private List<MovementSpeedModifier> movementSpeedModifiers = new List<MovementSpeedModifier>(); // a list of modifiers being applied to this enemy's movement speed 

    private float bonusSpeed = 1f;

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

        // enemy was stunned and cannot move
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
        state = EnemyState.Idle;

        agent.speed = enemyScriptableObject.movementSpeed * bonusSpeed;

    }

    private void OnDisable()
    {
        // reset any movement speed modifiers on the enemy
        bonusSpeed = 1f;
    }

    private void Update()
    {
        //agent.speed = enemyScriptableObject.movementSpeed;

        inAttackRange = Physics2D.OverlapCircle(transform.position, enemyScriptableObject.attackRange, enemyScriptableObject.attackLayer);
        inChaseRange = Physics2D.OverlapCircle(transform.position, enemyScriptableObject.chaseRange, enemyScriptableObject.chaseLayer);


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
        if (state != EnemyState.Death || state != EnemyState.Spawning)
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

    protected virtual void OnDeath()
    {
        gameObject.SetActive(false);

        Debug.Log("I am DEAD.");
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

    public void AddMovementSpeedModifier(MovementSpeedModifier modifierToAdd)
    {
        movementSpeedModifiers.Add(modifierToAdd);
        bonusSpeed += bonusSpeed * modifierToAdd.bonusMovementSpeed;
        agent.speed = enemyScriptableObject.movementSpeed * bonusSpeed;
    }

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove)
    {
        movementSpeedModifiers.Remove(modifierToRemove);
        bonusSpeed /= (1 + modifierToRemove.bonusMovementSpeed);

        agent.speed = enemyScriptableObject.movementSpeed * bonusSpeed;
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
