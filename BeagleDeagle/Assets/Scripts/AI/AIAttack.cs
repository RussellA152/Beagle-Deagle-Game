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
    [SerializeField]
    private ZombieAnimationHandler animationScript;

    [Header("Modifiers")]
    // A list of damage modifiers applied to the enemy's attack damage
    [SerializeField, NonReorderable]
    private List<DamageModifier> damageModifiers = new List<DamageModifier>();

    // A list of attack speed modifiers applied to the enemy's attack cooldown
    [SerializeField, NonReorderable]
    private List<AttackSpeedModifier> attackSpeedModifiers = new List<AttackSpeedModifier>();

    // A bonus percentage applied to the enemy's attack damage
    private float bonusDamage = 1f;

    // A bonus percentage applied to the enemy's attack cooldown
    private float bonusAttackSpeed = 1f;

    // Is the enemy allowed to attack?
    private bool canAttack = true;

    private void OnEnable()
    {
        canAttack = true;
    }


    public virtual void Attack(Transform target)
    {
        if (canAttack)
        {
            // TODO: This is temporary. Change to "whatEnemyAttacks" and GetComponent<IHealth>
            target.GetComponent<PlayerHealth>().ModifyHealth(enemyScriptableObject.attackDamage * bonusDamage);
            StartCoroutine(AttackCooldown());
        }
            
    }

    ///-///////////////////////////////////////////////////////////
    /// Wait some time to allow attacks again.
    /// Multiply by "bonusAttackSpeed" which can cause attacks to be slower or faster
    ///
    IEnumerator AttackCooldown()
    {
        // TODO: Call this function in a animation event (when the animation ends)
        canAttack = false;
        //Debug.Log("SWIPE AT TARGET!");

        yield return new WaitForSeconds(enemyScriptableObject.attackCooldown);

        canAttack = true;
    }

    ///-///////////////////////////////////////////////////////////
    /// Remove all damage and attackspeed modifiers from this enemy
    ///
    public void RevertAllModifiers()
    {
        // reset any modifiers on the enemy's damage
        bonusDamage = 1f;
        bonusAttackSpeed = 1f;

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
        bonusDamage += (bonusDamage * modifierToAdd.bonusDamage);

    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        damageModifiers.Remove(modifierToRemove);
        bonusDamage /= (1 + modifierToRemove.bonusDamage);
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        attackSpeedModifiers.Add(modifierToAdd);
        bonusAttackSpeed += (bonusAttackSpeed * modifierToAdd.bonusAttackSpeed);
        
        // Increase or decrease the animation speed of the movement animation
        animationScript.SetAttackAnimationSpeed(modifierToAdd.bonusAttackSpeed);
    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {
        attackSpeedModifiers.Remove(modifierToRemove);
        bonusAttackSpeed /= (1 + modifierToRemove.bonusAttackSpeed);
        
        animationScript.SetAttackAnimationSpeed(-1f * modifierToRemove.bonusAttackSpeed);
    }
}
