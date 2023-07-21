using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttack : MonoBehaviour, IEnemyDataUpdatable, IDamager
{
    [Header("Data to Use")]
    [SerializeField]
    private EnemyData enemyScriptableObject;
    
    [Header("Required Scripts")]
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

    private void Awake()
    {
        _animationScript = GetComponent<ZombieAnimationHandler>();
    }

    private void OnEnable()
    {
        _canAttack = true;
    }


    public virtual void Attack(Transform target)
    {
        if (_canAttack)
        {
            // TODO: This is temporary. Change to "whatEnemyAttacks" and GetComponent<IHealth>
            target.GetComponent<PlayerHealth>().ModifyHealth(enemyScriptableObject.attackDamage * _bonusDamage);
            StartCoroutine(AttackCooldown());
        }
            
    }

    ///-///////////////////////////////////////////////////////////
    /// Wait some time to allow attacks again.
    /// Multiply by "_bonusAttackSpeed" which can cause attacks to be slower or faster
    ///
    IEnumerator AttackCooldown()
    {
        // TODO: Call this function in a animation event (when the animation ends)
        _canAttack = false;
        //Debug.Log("SWIPE AT TARGET!");

        yield return new WaitForSeconds(enemyScriptableObject.attackCooldown);

        _canAttack = true;
    }

    ///-///////////////////////////////////////////////////////////
    /// Remove all damage and attackspeed modifiers from this enemy
    ///
    public void RevertAllModifiers()
    {
        // reset any modifiers on the enemy's damage
        _bonusDamage = 1f;
        _bonusAttackSpeed = 1f;

        // remove modifiers from lists
        damageModifiers.Clear();
        attackSpeedModifiers.Clear();
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        enemyScriptableObject = scriptableObject;
    }

    public void AddDamageModifier(DamageModifier modifierToAdd)
    {
        damageModifiers.Add(modifierToAdd);
        _bonusDamage += (_bonusDamage * modifierToAdd.bonusDamage);

    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        damageModifiers.Remove(modifierToRemove);
        _bonusDamage /= (1 + modifierToRemove.bonusDamage);
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        attackSpeedModifiers.Add(modifierToAdd);
        _bonusAttackSpeed += (_bonusAttackSpeed * modifierToAdd.bonusAttackSpeed);
        
        // Increase or decrease the animation speed of the movement animation
        _animationScript.SetAttackAnimationSpeed(modifierToAdd.bonusAttackSpeed);
    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {
        attackSpeedModifiers.Remove(modifierToRemove);
        _bonusAttackSpeed /= (1 + modifierToRemove.bonusAttackSpeed);
        
        _animationScript.SetAttackAnimationSpeed(-1f * modifierToRemove.bonusAttackSpeed);
    }
}
