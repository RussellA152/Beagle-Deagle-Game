using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttack : MonoBehaviour, IEnemyDataUpdatable, IDamager
{
    [SerializeField]
    private EnemyData enemyScriptableObject;

    [SerializeField, NonReorderable]
    private List<DamageModifier> damageModifiers = new List<DamageModifier>(); // a list of damage modifiers applied to the enemy's attack damage

    [SerializeField, NonReorderable]
    private List<AttackSpeedModifier> attackSpeedModifiers = new List<AttackSpeedModifier>(); // a list of attack speed modifiers applied to the enemy's attack cooldown

    private float bonusDamage = 1f; // a bonus percentage applied to the enemy's attack damage

    private float bonusAttackSpeed = 1f; // a bonus percentage applied to the enemy's attack cooldown

    private bool canAttack = true;

    private void OnEnable()
    {
        canAttack = true;
    }


    public virtual void Attack(Transform target)
    {
        if (canAttack)
        {
            target.GetComponent<PlayerHealth>().ModifyHealth(enemyScriptableObject.attackDamage * bonusDamage);
            StartCoroutine(AttackCooldown());
        }
            
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        Debug.Log("SWIPE AT TARGET!");

        yield return new WaitForSeconds(enemyScriptableObject.attackCooldown * bonusAttackSpeed);

        canAttack = true;
    }

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

    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {
        attackSpeedModifiers.Remove(modifierToRemove);
        bonusAttackSpeed /= (1 + modifierToRemove.bonusAttackSpeed);
    }
}
