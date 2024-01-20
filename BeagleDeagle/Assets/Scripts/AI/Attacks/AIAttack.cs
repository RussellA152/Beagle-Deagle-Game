using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIAttack<T> : MonoBehaviour, IEnemyDataUpdatable, IDamager, IHasCooldown, IRegisterModifierMethods where T: EnemyData
{
    [Header("Data to Use")]
    [SerializeField]
    protected T enemyScriptableObject;

    [Header("Required Scripts")] 
    private CooldownSystem _cooldownSystem;
    private ZombieAnimationHandler _animationScript;
    private ModifierManager _modifierManager;

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

        _modifierManager = GetComponent<ModifierManager>();

        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();

    }

    protected virtual void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = enemyScriptableObject.attackCooldown;
        
        BeginCooldown();
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
        if (!_cooldownSystem.IsOnCooldown(Id))
        {
            _cooldownSystem.PutOnCooldown(this);
            _canAttack = false;
        }
        
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
        if (damageModifiers.Contains(modifierToAdd)) return;
        
        damageModifiers.Add(modifierToAdd);
        _bonusDamage += (_bonusDamage * modifierToAdd.bonusDamage);
    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        if (!damageModifiers.Contains(modifierToRemove)) return;
        
        damageModifiers.Remove(modifierToRemove);
        _bonusDamage /= (1 + modifierToRemove.bonusDamage);
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        if (attackSpeedModifiers.Contains(modifierToAdd)) return;
        
        attackSpeedModifiers.Add(modifierToAdd);
        _bonusAttackSpeed += _bonusAttackSpeed * modifierToAdd.bonusAttackSpeed;
        
        // Increase or decrease the animation speed of the movement animation
        _animationScript.SetAttackAnimationSpeed(modifierToAdd.bonusAttackSpeed);
    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {
        if (!attackSpeedModifiers.Contains(modifierToRemove)) return;
        
        attackSpeedModifiers.Remove(modifierToRemove);
        _bonusAttackSpeed /= (1 + modifierToRemove.bonusAttackSpeed);
        
        _animationScript.SetAttackAnimationSpeed(-1f * modifierToRemove.bonusAttackSpeed);
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Remove all damage and attack speed modifiers from this enemy.
    ///
    public void RevertAllModifiers()
    {
        for (int i = damageModifiers.Count - 1; i >= 0; i--)
        {
            RemoveDamageModifier(damageModifiers[i]);
        }
        
        for (int i = attackSpeedModifiers.Count - 1; i >= 0; i--)
        {
            RemoveAttackSpeedModifier(attackSpeedModifiers[i]);
        }
    }

    #endregion
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<DamageModifier>(AddDamageModifier);
        _modifierManager.RegisterAddMethod<AttackSpeedModifier>(AddAttackSpeedModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<DamageModifier>(RemoveDamageModifier);
        _modifierManager.RegisterRemoveMethod<AttackSpeedModifier>(RemoveAttackSpeedModifier);
    }
}
