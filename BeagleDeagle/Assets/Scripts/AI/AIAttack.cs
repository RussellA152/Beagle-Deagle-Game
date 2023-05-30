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

    [SerializeField]
    private float bonusAttackSpeed = 1f; // a bonus percentage applied to the enemy's attack cooldown

    private bool canAttack = true;

    private void OnEnable()
    {
        canAttack = true;
    }

    private void OnDisable()
    {
        // reset any modifiers on the enemy's damage
        bonusDamage = 1f;
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

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        enemyScriptableObject = scriptableObject;
    }

    public void AddDamageModifier(DamageModifier modifierToAdd)
    {
        if (!damageModifiers.Contains(modifierToAdd))
        {
            damageModifiers.Add(modifierToAdd);
            bonusDamage += modifierToAdd.bonusDamage;
        }
        else
        {
            damageModifiers.Add(modifierToAdd);
        }

    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        if (!modifierToRemove.appliedOnTriggerEnter)
        {
            int count = damageModifiers.FindAll(num => num == modifierToRemove).Count;

            if (count > 1)
            {
                damageModifiers.Remove(modifierToRemove);
                Debug.Log("Keep slow effect!");
            }
            else if (count == 1)
            {
                damageModifiers.Remove(modifierToRemove);
                bonusDamage -= modifierToRemove.bonusDamage;

                Debug.Log("Remove slow effect permanently!");
            }
        }
        else
        {
            damageModifiers.Remove(modifierToRemove);
            bonusDamage -= modifierToRemove.bonusDamage;
        }
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        if (!attackSpeedModifiers.Contains(modifierToAdd))
        {
            attackSpeedModifiers.Add(modifierToAdd);
            bonusAttackSpeed += modifierToAdd.bonusAttackSpeed;
        }
        else
        {
            attackSpeedModifiers.Add(modifierToAdd);
        }

    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {

        if (!modifierToRemove.appliedOnTriggerEnter)
        {
            int count = attackSpeedModifiers.FindAll(num => num == modifierToRemove).Count;

            if (count > 1)
            {
                attackSpeedModifiers.Remove(modifierToRemove);
                Debug.Log("Keep slow effect!");
            }
            else if (count == 1)
            {
                attackSpeedModifiers.Remove(modifierToRemove);
                bonusAttackSpeed -= modifierToRemove.bonusAttackSpeed;

                Debug.Log("Remove slow effect permanently!");
            }
        }
        else
        {
            attackSpeedModifiers.Remove(modifierToRemove);
            bonusAttackSpeed -= modifierToRemove.bonusAttackSpeed;
        }
        
    }
}
