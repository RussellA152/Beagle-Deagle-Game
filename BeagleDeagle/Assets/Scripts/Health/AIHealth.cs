using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour, IHealth, IEnemyDataUpdatable
{
    [SerializeField] private EnemyEvents enemyEvents;
    
    [Header("Data to Use")]
    [SerializeField] private EnemyData enemyData;
    

    [Header("Modifiers")]
    [SerializeField, NonReorderable]
    private List<MaxHealthModifier> maxHealthModifiers = new List<MaxHealthModifier>(); // display all modifiers applied to the bonusMaxHealth (for debugging mainly)
    
    // a bonus percentage applied to the enemy's max health (Ex. 500 max health * 120%, would mean 120% extra max health)
    private float _bonusMaxHealth = 1f;
    
    // The current health of this enemy
    private float _currentHealth;
    
    // Is the enemy currently dead?
    private bool _isDead;

    protected virtual void OnEnable()
    {
        _isDead = false;
        _currentHealth = enemyData.maxHealth;
    }

    public virtual float GetCurrentHealth()
    {
        return _currentHealth;
    }


    public virtual void ModifyHealth(float amount)
    {
        // Calculate the potential new health value
        float newHealth = _currentHealth + amount;

        // Clamp the new health value between 0 and the maximum potential health (including any max health modifiers)
        newHealth = Mathf.Clamp(newHealth, 0f, enemyData.maxHealth * _bonusMaxHealth);

        // Check if the new health value is zero or below
        if (newHealth <= 0f)
        {
            _currentHealth = 0f;
            _isDead = true;
            
            // Give the player a certain amount of xp upon death
            enemyEvents.InvokeGiveXp(enemyData.xpOnDeath);
        }
        else
        {
            _currentHealth = newHealth;
        }
    }
    
    public bool IsDead()
    {
        return _isDead;
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        enemyData = scriptableObject;
    }

    #region HealthModifiers
    public void AddMaxHealthModifier(MaxHealthModifier modifierToAdd)
    {
        maxHealthModifiers.Add(modifierToAdd);
        _bonusMaxHealth += modifierToAdd.bonusMaxHealth;
    }

    public void RemoveMaxHealthModifier(MaxHealthModifier modifierToRemove)
    {
        maxHealthModifiers.Remove(modifierToRemove);
        _bonusMaxHealth /= (1 + modifierToRemove.bonusMaxHealth);
    }

    public void RevertAllModifiers()
    {
        // reset any max health modifiers applied to an enemy
        _bonusMaxHealth = 1f;
        maxHealthModifiers.Clear();
    }
    #endregion
    
}
