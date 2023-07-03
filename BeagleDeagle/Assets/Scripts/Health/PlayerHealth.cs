using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth, IPlayerDataUpdatable
{
    [SerializeField]
    private PlayerEvents playerEvents;

    //private IPlayerStatModifier playerStatModifierScript;

    private float bonusMaxHealth = 1f; // a bonus percentage applied to the player's max health (Ex. 500 max health * 120%, would mean 120% extra max health)

    private float currentHealth;
    //private float maxHealth; // we make a local variable in here so that we don't affect original SO data
    private bool isDead;

    [SerializeField]
    private CharacterData playerData;

    [SerializeField, NonReorderable]
    private List<MaxHealthModifier> maxHealthModifiers = new List<MaxHealthModifier>(); // display all modifiers applied to the bonusMaxHealth (for debugging mainly)

    //[SerializeField, NonReorderable]
    //private List<DamageOverTime> damageOverTimeEffects = new List<DamageOverTime>(); // All DOT's that have been applied to the player

    private void Start()
    {
        InitializeHealth();

    }

    public void InitializeHealth()
    {
        isDead = false;

        currentHealth = playerData.maxHealth * bonusMaxHealth;

        playerEvents.InvokeCurrentHealthEvent(currentHealth);

        playerEvents.InvokeMaxHealthEvent(playerData.maxHealth * bonusMaxHealth);
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
        newHealth = Mathf.Clamp(newHealth, 0f, playerData.maxHealth * bonusMaxHealth);

        // Check if the new health value is zero or below
        if (newHealth <= 0f)
        {
            currentHealth = 0f;
            playerEvents.InvokeCurrentHealthEvent(currentHealth);
            isDead = true;
        }
        else
        {
            currentHealth = newHealth;
            playerEvents.InvokeCurrentHealthEvent(currentHealth);
        }
    }


    public void MaxHealthWasModified()
    {
        // Invoke max health event
        playerEvents.InvokeMaxHealthEvent(playerData.maxHealth * bonusMaxHealth);
    }

    // do something when this entity dies
    public bool IsDead()
    {
        return isDead;
    }

    public void UpdateScriptableObject(CharacterData scriptableObject)
    {
        playerData = scriptableObject;

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
}
