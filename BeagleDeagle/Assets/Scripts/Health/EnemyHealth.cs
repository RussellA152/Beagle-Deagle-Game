using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth, IHealthWithModifiers,IEnemyDataUpdatable, IRegisterModifierMethods
{
    [SerializeField] private CurrencyEvents currencyEvents;
    
    [Header("Data to Use")]
    [SerializeField] private EnemyData enemyData;

    private ModifierManager _modifierManager;
    

    [Header("Modifiers")]
    [SerializeField, NonReorderable]
    private List<MaxHealthModifier> maxHealthModifiers = new List<MaxHealthModifier>(); // display all modifiers applied to the bonusMaxHealth (for debugging mainly)
    
    // a bonus percentage applied to the enemy's max health (Ex. 500 max health * 120%, would mean 120% extra max health)
    private float _bonusMaxHealth = 1f;
    
    // The current health of this enemy
    private float _currentHealth;
    
    public event Action onDeath;
    private bool _isDead;

    private void Awake()
    {
        _modifierManager = GetComponent<ModifierManager>();
        
        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();
    }

    protected virtual void OnEnable()
    {
        _currentHealth = enemyData.maxHealth;
        _isDead = false;
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
            
            InvokeDeathEvent();
            
            _isDead = true;
            
            // Give the player a certain amount of xp upon death
            currencyEvents.InvokeGiveXp(enemyData.currencyRewardOnDeath.xpAmount);
            // Tell other scripts that this enemy has died
            EnemyManager.Instance.InvokeEnemyDeathGiveGameObject(gameObject);
        }
        else
        {
            _currentHealth = newHealth;
        }
    }
    
    public void InvokeDeathEvent()
    {
        onDeath?.Invoke();
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        enemyData = scriptableObject;
        
        _currentHealth = enemyData.maxHealth;
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
        for (int i = maxHealthModifiers.Count - 1; i >= 0; i--)
        {
            RemoveMaxHealthModifier(maxHealthModifiers[i]);
        }
        
    }
    #endregion

    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<MaxHealthModifier>(AddMaxHealthModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<MaxHealthModifier>(RemoveMaxHealthModifier);
    }
}
