using System;
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

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = enemyData.maxHealth;
    }

    public virtual float GetCurrentHealth()
    {
        return currentHealth;
    }


    public virtual void ModifyHealth(float amount)
    {
        // Calculate the potential new health value
        float newHealth = currentHealth + amount;

        // Clamp the new health value between 0 and the maximum potential health (including any max health modifiers)
        newHealth = Mathf.Clamp(newHealth, 0f, enemyData.maxHealth * bonusMaxHealth);

        // Check if the new health value is zero or below
        if (newHealth <= 0f)
        {
            currentHealth = 0f;
            isDead = true;
        }
        else
        {
            currentHealth = newHealth;
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
    }

}
