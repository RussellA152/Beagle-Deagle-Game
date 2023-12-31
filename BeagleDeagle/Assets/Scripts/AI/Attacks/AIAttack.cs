using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIAttack<T> : MonoBehaviour, IEnemyDataUpdatable, IDamager, IHasCooldown where T: EnemyData
{
    [Header("Data to Use")]
    [SerializeField]
    protected T enemyScriptableObject;

    [Header("Required Scripts")] 
    private CooldownSystem _cooldownSystem;
    private ZombieAnimationHandler _animationScript;

    [Header("Modifiers")]
    // A list of damage modifiers applied to the enemy's attack damage
    [SerializeField, NonReorderable]
    private List<DamageModifier> damageModifiers = new List<DamageModifier>();

    // A list of attack speed modifiers applied to the enemy's attack cooldown
    [SerializeField, NonReorderable]
    private List<AttackSpeedModifier> attackSpeedModifiers = new List<AttackSpeedModifier>();

    // A bonus percentage applied to the enemy's attack damage
    private float _bonusDamage = 1f;

    // A bonus percentage applied to the enemy's attack cooldown
    private float _bonusAttackSpeed = 1f;

    // Is the enemy allowed to attack?
    private bool _canAttack = true;
    
    // The object the enemy wants to attack (usually player)
    protected Transform Target;

    private void Awake()
    {
        _animationScript = GetComponent<ZombieAnimationHandler>();
        _cooldownSystem = GetComponent<CooldownSystem>();
        
    }

    protected virtual void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = enemyScriptableObject.attackCooldown;
    }

    protected virtual void OnEnable()
    {
        // When cooldown ends, allow attack
        _cooldownSystem.OnCooldownEnded += OnAttackCooldownFinish;

        BeginCooldown();
    }

    private void OnDisable()
    {
        _cooldownSystem.OnCooldownEnded -= OnAttackCooldownFinish;
    }


    ///-///////////////////////////////////////////////////////////
    /// Perform an attack against the target
    /// 
    public abstract void InitiateAttack();

    ///-///////////////////////////////////////////////////////////
    /// When the enemy is finished with their attack animation, begin their attack cooldown.
    /// 
    public void BeginCooldown()
    {
        _cooldownSystem.PutOnCooldown(this);
        _canAttack = false;
    }

    ///-///////////////////////////////////////////////////////////
    /// Allow the enemy to attack again when the attack cooldown finishes
    /// 
    public void OnAttackCooldownFinish(int id)
    {
        if (id == Id)
        {
            _canAttack = true;
        }
    }
    
    public bool GetCanAttack()
    {
        return _canAttack;
    }

    ///-///////////////////////////////////////////////////////////
    /// Remove all damage and attackspeed modifiers from this enemy
    ///
    public void RevertAllModifiers()
    {
        foreach (DamageModifier damageModifier in damageModifiers)
        {
            RemoveDamageModifier(damageModifier);
        }

        foreach (AttackSpeedModifier attackSpeedModifier in attackSpeedModifiers)
        {
            RemoveAttackSpeedModifier(attackSpeedModifier);
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
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

    #region AttackModifiers
    public void AddDamageModifier(DamageModifier modifierToAdd)
    {
        damageModifiers.Add(modifierToAdd);
        _bonusDamage += (_bonusDamage * modifierToAdd.bonusDamage);

        //modifierToAdd.isActive = true;
    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        damageModifiers.Remove(modifierToRemove);
        _bonusDamage /= (1 + modifierToRemove.bonusDamage);
        
        //modifierToRemove.isActive = false;
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        attackSpeedModifiers.Add(modifierToAdd);
        _bonusAttackSpeed += (_bonusAttackSpeed * modifierToAdd.bonusAttackSpeed);
        
        // Increase or decrease the animation speed of the movement animation
        _animationScript.SetAttackAnimationSpeed(modifierToAdd.bonusAttackSpeed);
        
        //modifierToAdd.isActive = true;
    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {
        attackSpeedModifiers.Remove(modifierToRemove);
        _bonusAttackSpeed /= (1 + modifierToRemove.bonusAttackSpeed);
        
        _animationScript.SetAttackAnimationSpeed(-1f * modifierToRemove.bonusAttackSpeed);

        //modifierToRemove.isActive = false;
    }

    #endregion
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
