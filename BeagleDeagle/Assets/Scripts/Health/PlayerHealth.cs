using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth, IHealthWithModifiers, IPlayerDataUpdatable, IHasCooldown, IRegisterModifierMethods
{
    [SerializeField] private PlayerEvents playerEvents;
    
    [SerializeField] private PlayerData playerData;
    
    private CooldownSystem _cooldownSystem;
    
    private ModifierManager _modifierManager;

    private float _bonusMaxHealth = 1f; // a bonus percentage applied to the player's max health (Ex. 500 max health * 120%, would mean 120% extra max health)

    private float _currentHealth;
    
    [Range(0.5f, 15f)] public float healthRegenDelay = 1f;
    
    private bool _isDead;

    public event Action onDeath;
    
    public event Action onTookDamage;

    [SerializeField, NonReorderable]
    private List<MaxHealthModifier> maxHealthModifiers = new List<MaxHealthModifier>(); // display all modifiers applied to the bonusMaxHealth (for debugging mainly)

    private void Awake()
    {
        _cooldownSystem = GetComponent<CooldownSystem>();
        _modifierManager = GetComponent<ModifierManager>();
        
        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();
        
        
    }

    private void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        
        _isDead = false;

        // After getting hit, how does it take to start regenerating health?
        CooldownDuration = healthRegenDelay;

        // Passives occur in start, so put this in on enable
        _currentHealth = playerData.maxHealth * _bonusMaxHealth;
        
        // Tell all listeners the value of the player's current and max health
        playerEvents.InvokeCurrentHealthEvent(_currentHealth);
        playerEvents.InvokeMaxHealthEvent(playerData.maxHealth * _bonusMaxHealth);
        
    }
    
    private void Update()
    {
        // Regenerate health while player's health is below a certain percentage
        if (!_isDead && IsHealthBelowPercentage(playerData.healthRegenData.regenThreshold) && !_cooldownSystem.IsOnCooldown(Id))
            ModifyHealth(playerData.healthRegenData.regenRate * Time.deltaTime);
    }

    public virtual void ModifyHealth(float amount)
    {
        // Calculate the potential new health value
        float newHealth = _currentHealth + amount;

        // Clamp the new health value between 0 and the maximum potential health (including any max health modifiers)
        newHealth = Mathf.Clamp(newHealth, 0f, playerData.maxHealth * _bonusMaxHealth);
        

        // Check if the new health value is zero or below
        if (newHealth <= 0f)
        {
            _currentHealth = 0f;
            _isDead = true;
            
            // Tell all listeners that the player has died
            InvokeDeathEvent();
        }
        else
        {
            // Player took damage 
            if (newHealth < _currentHealth)
            {
                InvokeTookDamageEvent();
                
                // Place cooldown on health regeneration
                // Reset cooldown if player was hit while regenerating
                if(!_cooldownSystem.IsOnCooldown(Id))
                    _cooldownSystem.PutOnCooldown(this);
                else
                    _cooldownSystem.RefreshCooldown(Id);
                
            }
            _currentHealth = newHealth;
        }
        
            // Current health has changed, so update all listeners with the new value
        playerEvents.InvokeCurrentHealthEvent(_currentHealth);
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public bool IsHealthBelowPercentage(float healthPercentage)
    {
        return (_currentHealth < ((playerData.maxHealth * _bonusMaxHealth) * healthPercentage));
    }
    
    public void InvokeTookDamageEvent()
    {
        playerEvents.InvokePlayerTookDamage();
        onTookDamage?.Invoke();
    }

    public bool IsDead()
    {
        return _isDead;
    }

    // Do something when this entity dies
    public void InvokeDeathEvent()
    {
        playerEvents.InvokePlayerDied();
        onDeath?.Invoke();
    }

    public void UpdateScriptableObject(PlayerData scriptableObject)
    {
        playerData = scriptableObject;

    }

    #region HealthModifiers
    public void AddMaxHealthModifier(MaxHealthModifier modifierToAdd)
    {
        maxHealthModifiers.Add(modifierToAdd);
        _bonusMaxHealth += modifierToAdd.bonusMaxHealth;

        _currentHealth = playerData.maxHealth * _bonusMaxHealth;
        
        playerEvents.InvokeCurrentHealthEvent(_currentHealth);
        playerEvents.InvokeMaxHealthEvent(playerData.maxHealth * _bonusMaxHealth);
        
    }

    public void RemoveMaxHealthModifier(MaxHealthModifier modifierToRemove)
    {
        maxHealthModifiers.Remove(modifierToRemove);
        _bonusMaxHealth /= (1 + modifierToRemove.bonusMaxHealth);
        
        _currentHealth = playerData.maxHealth * _bonusMaxHealth;
        
        playerEvents.InvokeCurrentHealthEvent(_currentHealth);
        playerEvents.InvokeMaxHealthEvent(playerData.maxHealth * _bonusMaxHealth);

    }
    #endregion

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<MaxHealthModifier>(AddMaxHealthModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<MaxHealthModifier>(RemoveMaxHealthModifier);
    }
}
