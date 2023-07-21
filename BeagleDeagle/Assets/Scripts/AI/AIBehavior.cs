using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

///-///////////////////////////////////////////////////////////
/// Responsible for executing code based on enemy states. States include: Idle, Chasing, Attack, Stunned, and Death.
/// This script holds references to the enemy's attack, movement, and health scripts.
/// 
public abstract class AIBehavior<T> : MonoBehaviour, IPoolable, IEnemyDataUpdatable where T: EnemyData
{
    private EnemyState _state;
    
    [SerializeField]
    private int poolKey;

    [Header("Data to Use")]
    public T enemyScriptableObject;
    
    [Header("Enemy's Target")]
    [SerializeField] // TEMPORARY, WILL NEED A DIFFERENT WAY TO REFERENCE THE PLAYER *
    private Transform target; // Who this enemy will chase and attack?
    
    [Header("Required Components")]
    private NavMeshAgent agent;
    private Collider2D bodyCollider;

    [Header("Required Scripts")]
    private AIAttack attackScript;
    private AIMovement movementScript;
    private AIHealth healthScript;
    private DamageOverTimeHandler damageOverTimeScript;
    private ZombieAnimationHandler animationHandlerScript;
    

    private bool _inAttackRange; // Is the player within this enemy's attack range?
    private bool _inChaseRange; // Is the player within this enemy's chase/follow range?

    public int PoolKey => poolKey;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Prevents AI from spawning with incorrect rotation with NavMeshPlus (2D navmesh asset)
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        bodyCollider = GetComponent<Collider2D>();
        
        attackScript = GetComponent<AIAttack>();
        movementScript = GetComponent<AIMovement>();
        healthScript = GetComponent<AIHealth>();
        damageOverTimeScript = GetComponent<DamageOverTimeHandler>();
        animationHandlerScript = GetComponent<ZombieAnimationHandler>();

    }

    ///-///////////////////////////////////////////////////////////
    /// All states that an enemy can be in at once
    /// 
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
    protected virtual void Start()
    {
        _state = EnemyState.Idle;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    private void OnEnable()
    {
        // When enemy spawns in, they start as Idle
        _state = EnemyState.Idle;
        
        // Re-enable the enemy's body collider (we disable it when they die)
        bodyCollider.enabled = true;

    }

    private void Update()
    {
        // Check if player is within chasing range
        _inChaseRange = Physics2D.OverlapCircle(transform.position, enemyScriptableObject.chaseRange, enemyScriptableObject.chaseLayer);

        // Only check for attack range if the player is within chase range
        if (_inChaseRange)
            _inAttackRange = Physics2D.OverlapCircle(transform.position, enemyScriptableObject.attackRange, enemyScriptableObject.attackLayer);
       

        // Update this enemy's state
        CheckState();

        switch (_state)
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

    ///-///////////////////////////////////////////////////////////
    /// Check if the enemy meets conditions to change to a new state.
    /// For instance, if the enemy is in chase range and not in attack range,
    /// then switch to chase state
    /// 
    protected virtual void CheckState()
    {
        if (healthScript.IsDead())
        {
            _state = EnemyState.Death;
            return;
        }

        if (movementScript.IsStunned)
        {
            _state = EnemyState.Stunned;
            return;
        }

        // If the enemy is not dead or currently spawning...
        if (_state != EnemyState.Death || _state != EnemyState.Spawning)
        {
            // In chase range, but not in attack range
            if (_inChaseRange && !_inAttackRange)
                _state = EnemyState.Chase;

            // In attack range
            else if (_inChaseRange && _inAttackRange)
                _state = EnemyState.Attack;

            // Go idle if not in chase range and not in attack range
            else if (!_inChaseRange && !_inAttackRange)
                _state = EnemyState.Idle;
        }
    }
    protected virtual void OnIdle()
    {
        animationHandlerScript.PlayIdleAnimation();
        
        agent.isStopped = true;
    }
    protected virtual void OnChase()
    {
        animationHandlerScript.PlayMoveAnimation();
        
        agent.isStopped = false;

        if (target != null)
            agent.SetDestination(target.position);
    }

    protected virtual void OnAttack()
    {
        agent.isStopped = true;

        animationHandlerScript.PlayAttackAnimation();
        attackScript.Attack(target);
    }
    
    protected virtual void OnStun()
    {
        animationHandlerScript.PlayStunAnimation();
        agent.velocity = Vector2.zero;
        agent.isStopped = true;
    }
    
    protected virtual void OnDeath()
    {
        animationHandlerScript.PlayDeathAnimation();
        
        // Don't let enemy move, and disable their collider
        agent.isStopped = true;

        bodyCollider.enabled = false;
        
        RevertAllModifiersOnEnemy();

        //Debug.Log("I am DEAD.");
    }

    ///-///////////////////////////////////////////////////////////
    /// An animation event will call this function to
    /// disable this enemy's corpse after their death animation has finished
    /// 
    public void DisableCorpse()
    {
        gameObject.SetActive(false);
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Remove all modifiers when this enemy dies or their gameObject gets disabled
    /// 
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

    public virtual void UpdateScriptableObject(EnemyData scriptableObject)
    {
        if (scriptableObject is T)
        {
            enemyScriptableObject = scriptableObject as T;
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }

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
