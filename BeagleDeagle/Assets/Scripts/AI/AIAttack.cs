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

    private float bonusDamage = 1f; // a bonus percentage applied to the enemy's attack damage

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

        yield return new WaitForSeconds(enemyScriptableObject.attackCooldown);

        canAttack = true;
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        enemyScriptableObject = scriptableObject;
    }

    public void AddDamageModifier(DamageModifier modifierToAdd)
    {
        damageModifiers.Add(modifierToAdd);
        bonusDamage += modifierToAdd.bonusDamage;
    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        damageModifiers.Remove(modifierToRemove);
        bonusDamage -= modifierToRemove.bonusDamage;
    }
}
