using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth, IHealthWIthModifiers, IPlayerDataUpdatable, IHasCooldown
{
    [SerializeField] private PlayerEvents playerEvents;
    
    [SerializeField] private PlayerData playerData;

    [SerializeField] private float healthRegenDelay = 1f;

    private CooldownSystem _cooldownSystem;

    private float _bonusMaxHealth = 1f; // a bonus percentage applied to the player's max health (Ex. 500 max health * 120%, would mean 120% extra max health)

    private float _currentHealth;
    
    private bool _isDead;

    [SerializeField, NonReorderable]
    private List<MaxHealthModifier> maxHealthModifiers = new List<MaxHealthModifier>(); // display all modifiers applied to the bonusMaxHealth (for debugging mainly)

    private void Awake()
    {
        _cooldownSystem = GetComponent<CooldownSystem>();
    }

    private void Start()
    {
        Id = 8;
        
        _isDead = false;

        // After getting hit, how does it take to start regenerating health?
        CooldownDuration = healthRegenDelay;

        _currentHealth = playerData.maxHealth * _bonusMaxHealth;

        // Tell all listeners the value of the player's current and max health
        playerEvents.InvokeCurrentHealthEvent(_currentHealth);
        playerEvents.InvokeMaxHealthEvent(playerData.maxHealth * _bonusMaxHealth);

    }

    private void Update()
    {
        if (_currentHealth < (playerData.maxHealth * _bonusMaxHealth) * playerData.healthRegenPercentage && !_cooldownSystem.IsOnCooldown(Id))
        {
            ModifyHealth(playerData.regenRate * Time.deltaTime);

        }
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
            playerEvents.InvokePlayerDied();
        }
        else
        {
            // Player took damage 
            if (newHealth < _currentHealth)
            {
                playerEvents.InvokePlayerTookDamage();
                
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

    // Do something when this entity dies
    public bool IsDead()
    {
        return _isDead;
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

        //modifierToAdd.isActive = true;
    }

    public void RemoveMaxHealthModifier(MaxHealthModifier modifierToRemove)
    {
        maxHealthModifiers.Remove(modifierToRemove);
        _bonusMaxHealth /= (1 + modifierToRemove.bonusMaxHealth);

        //modifierToRemove.isActive = false;
    }
    #endregion

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
