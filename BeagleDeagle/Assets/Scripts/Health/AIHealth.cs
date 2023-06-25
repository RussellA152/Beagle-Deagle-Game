using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour, IHealth, IEnemyDataUpdatable
{
    private float currentHealth;
    private bool isDead;

    [SerializeField]
    private EnemyData enemyData;

    private float bonusMaxHealth = 1f; // a bonus percentage applied to the enemy's max health (Ex. 500 max health * 120%, would mean 120% extra max health)

    [SerializeField, NonReorderable]
    private List<MaxHealthModifier> maxHealthModifiers = new List<MaxHealthModifier>(); // display all modifiers applied to the bonusMaxHealth (for debugging mainly)

    [SerializeField, NonReorderable]
    private List<DamageOverTime> damageOverTimeEffects = new List<DamageOverTime>(); // All DOT's that have been applied to the enemy

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = enemyData.maxHealth;
    }

    public virtual float GetCurrentHealth()
    {
        return currentHealth;
    }


    // add or subtract from health count
    public virtual void ModifyHealth(float amount)
    {
        // if this health modification will exceed the max potential health, then just set the current health to max
        if (currentHealth + amount > enemyData.maxHealth * bonusMaxHealth)
        {
            currentHealth = enemyData.maxHealth * bonusMaxHealth;
        }

        // if this health modification will drop the health to 0 or below, then call OnDeath()
        else if (currentHealth + amount <= 0f)
        {
            currentHealth = 0;
            isDead = true;
        }

        else
        {
            currentHealth += amount;
        }

    }

    // do something when this entity dies
    public bool IsDead()
    {
        return isDead;
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        enemyData = scriptableObject;
    }

    public void AddMaxHealthModifier(MaxHealthModifier modifierToAdd)
    {
        maxHealthModifiers.Add(modifierToAdd);
        bonusMaxHealth += modifierToAdd.bonusMaxHealth;
    }

    public void RemoveMaxHealthModifier(MaxHealthModifier modifierToRemove)
    {
        maxHealthModifiers.Remove(modifierToRemove);
        bonusMaxHealth /= (1 + modifierToRemove.bonusMaxHealth);
    }

    public void RevertAllModifiers()
    {
        // reset any max health modifiers applied to an enemy
        bonusMaxHealth = 1f;
        maxHealthModifiers.Clear();
        damageOverTimeEffects.Clear();
    }

    public void AddDamageOverTime(DamageOverTime dotToAdd)
    {
        damageOverTimeEffects.Add(dotToAdd);

        StartCoroutine(TakeDamageOverTime(dotToAdd));
    }

    public void RemoveDamageOverTime(DamageOverTime dotToRemove)
    {
        damageOverTimeEffects.Remove(dotToRemove);

    }

    public IEnumerator TakeDamageOverTime(DamageOverTime dot)
    {
        float ticks = dot.ticks;

        while(ticks > 0)
        {
            // THIS ASSUMES WE ALWAYS DO DAMAGE! WILL CHANGE!
            ModifyHealth(-1f * dot.damage);

            yield return new WaitForSeconds(dot.tickInterval);

            ticks--;
        }

        RemoveDamageOverTime(dot);
    }
}
