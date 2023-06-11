using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth, IPlayerDataUpdatable
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    //private IPlayerStatModifier playerStatModifierScript;

    private float bonusMaxHealth = 1f; // a bonus percentage applied to the player's max health (Ex. 500 max health * 120%, would mean 120% extra max health)

    private float currentHealth;
    //private float maxHealth; // we make a local variable in here so that we don't affect original SO data
    private bool isDead;

    [SerializeField]
    private PlayerData playerData;

    [SerializeField, NonReorderable]
    private List<MaxHealthModifier> maxHealthModifiers = new List<MaxHealthModifier>(); // display all modifiers applied to the bonusMaxHealth (for debugging mainly)

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

    // add or subtract from health count
    public virtual void ModifyHealth(float amount)
    {
        // If this health modification will exceed the max potential health, then just set the current health to max.
        // We add the maxHealth by the playerStat's modifier just in case the player has any items that affect their max health (Ex. Health upgrade passive)
        if (currentHealth + amount > playerData.maxHealth * bonusMaxHealth)
        {
            currentHealth = playerData.maxHealth * bonusMaxHealth;
            playerEvents.InvokeCurrentHealthEvent(currentHealth);
        }

        // if this health modification will drop the health to 0 or below, then call OnDeath()
        else if (currentHealth + amount <= 0f)
        {
            currentHealth = 0;
            playerEvents.InvokeCurrentHealthEvent(currentHealth);
            isDead = true;
        }

        else
        {
            currentHealth += amount;
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

    public void UpdateScriptableObject(PlayerData scriptableObject)
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
