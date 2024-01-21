using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour, IMovable, IStunnable, IKnockbackable, IEnemyDataUpdatable, IRegisterModifierMethods
{
    [Header("Data to Use")]
    [SerializeField] private EnemyData enemyScriptableObject; // Data for enemy's movement (contains movement speed)
    
    [Header("Required Components")]
    private NavMeshAgent _agent;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private ModifierManager _modifierManager;
    private ModifierParticleEffectHandler _modifierParticleEffectHandler;

    [Header("Required Scripts")]
    private ZombieAnimationHandler _animationScript;

    [Header("Modifiers")]
    [SerializeField, NonReorderable] private List<MovementSpeedModifier> movementSpeedModifiers = new List<MovementSpeedModifier>(); // a list of modifiers being applied to this enemy's movement speed 
    private float _bonusSpeed = 1f; // Speed multiplier on the movement speed of this enemy
    
    public bool IsStunned { get; private set; } // Is this enemy stunned?
    
    // Can the enemy turn around?
    private bool _canFlip = true;
    
    private Transform _target;
    
    // The x scale that the enemy was instantiated with
    //private float _originalTransformScaleX;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody2D>();
        
        // Sprite renderer is on a child gameObject on the player
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        _animationScript = GetComponent<ZombieAnimationHandler>();
        _modifierManager = GetComponent<ModifierManager>();

        _modifierParticleEffectHandler = GetComponent<ModifierParticleEffectHandler>();
        
        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();
    }
    

    private void OnEnable()
    {
        IsStunned = false;
        
        // Set the speed of the enemy
        _agent.speed = enemyScriptableObject.movementSpeed * _bonusSpeed;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        FlipSprite();

    }
    
    ///-///////////////////////////////////////////////////////////
    /// When this enemy's target is to their left, flip their sprite to the left.
    /// Otherwise, keep their sprite facing right.
    /// 
    private void FlipSprite()
    {
        if (_canFlip)
        {
            if (_target.position.x < transform.position.x)
                transform.localScale = new Vector3(-1f * enemyScriptableObject.scaleSize.x, transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(enemyScriptableObject.scaleSize.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    public void SetCanFlip(bool boolean)
    {
        _canFlip = boolean;
    }

    #region Stun
    
    ///-///////////////////////////////////////////////////////////
    /// Disable the enemy's ability to move.
    /// Then start a coroutine to wait some time and remove the stun effect
    /// 
    public void GetStunned(StunModifier stunModifier)
    {
        IsStunned = true;

    }
    
    public void RemoveStun(StunModifier stunModifier)
    {
        IsStunned = false;
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Wait some time, then remove stun from enemy
    /// 
    // public IEnumerator RemoveStunCoroutine(StunModifier stunModifier)
    // {
    //     IsStunned = true;
    //
    //     yield return new WaitForSeconds(stunModifier.stunDuration);
    //     
    //     RemoveStun(stunModifier);
    //
    // }
    
    
    #endregion

    #region Knockback
    ///-///////////////////////////////////////////////////////////
    /// Apply a specified amount of force in a direction to the enemy.
    /// 
    public void ApplyKnockBack(Vector2 force, Vector2 direction)
    {
        // Prevent enemy from moving
        // Also, this enemy will not be able to flip their sprite because they are stopped
        _agent.isStopped = true;

        _rb.AddForce(direction * force, ForceMode2D.Impulse);

        StartCoroutine(WaitForKnockBackToFinish());
    }


    ///-///////////////////////////////////////////////////////////
    /// Wait until the enemy's velocity reaches near zero to allow them to move again
    /// 
    public IEnumerator WaitForKnockBackToFinish()
    {
        // Wait for a short duration before checking
        yield return new WaitForSeconds(0.1f);

        // Continue waiting until velocity becomes small
        while (_rb.velocity.magnitude > 0.1f) 
        {
            yield return null;
        }

        // Allow enemy to move again
        _agent.isStopped = false;
    }
    

    #endregion

    #region MovementModifiers
    public void AddMovementSpeedModifier(MovementSpeedModifier modifierToAdd)
    {
        if (movementSpeedModifiers.Contains(modifierToAdd)) return;
        
        movementSpeedModifiers.Add(modifierToAdd);
        _bonusSpeed += _bonusSpeed * modifierToAdd.bonusMovementSpeed;
        _agent.speed = enemyScriptableObject.movementSpeed * _bonusSpeed;
        
        //_modifierParticleEffectHandler.StartPlayingParticle(modifierToAdd, true);
        
        // Increase or decrease the animation speed of the movement animation
        _animationScript.SetMovementAnimationSpeed(modifierToAdd.bonusMovementSpeed);
        
    }

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove)
    {
        if (!movementSpeedModifiers.Contains(modifierToRemove)) return;
        
       // _modifierParticleEffectHandler.StopSpecificParticle(modifierToRemove);
        
        movementSpeedModifiers.Remove(modifierToRemove);
        _bonusSpeed /= (1 + modifierToRemove.bonusMovementSpeed);
        _agent.speed = enemyScriptableObject.movementSpeed * _bonusSpeed;

        // Remove speed effect that was applied to the movement animation's speed
        _animationScript.SetMovementAnimationSpeed(-1f * modifierToRemove.bonusMovementSpeed);
        
    }

    public void RevertAllModifiers()
    {
        // Remove speed modifiers from list when spawning
        for (int i = movementSpeedModifiers.Count - 1; i >= 0; i--)
        {
            RemoveMovementSpeedModifier(movementSpeedModifiers[i]);
        }
    }

    #endregion
    
    public void AllowMovement(bool boolean)
    {
        if (boolean)
        {
            _agent.isStopped = false;
        }
        else
        {
            _agent.isStopped = true;
        }
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        enemyScriptableObject = scriptableObject;
        _agent.speed = enemyScriptableObject.movementSpeed * _bonusSpeed;
    }

    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<MovementSpeedModifier>(AddMovementSpeedModifier);
        _modifierManager.RegisterAddMethod<StunModifier>(GetStunned);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<MovementSpeedModifier>(RemoveMovementSpeedModifier);
        _modifierManager.RegisterRemoveMethod<StunModifier>(RemoveStun);
    }
}
