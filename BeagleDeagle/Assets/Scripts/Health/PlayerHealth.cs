using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth, IPlayerDataUpdatable
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    private float currentHealth;
    private float maxHealth; // we make a local variable in here so that we don't affect original SO data
    private bool isDead;

    [SerializeField]
    private PlayerData playerData;

    //protected virtual void OnEnable()
    //{
    //    isDead = false;
    //    currentHealth = playerData.maxHealth;
    //    maxHealth = playerData.maxHealth;


    //}

    public void InitializeHealth()
    {
        isDead = false;
        currentHealth = playerData.maxHealth;
        maxHealth = playerData.maxHealth;

        playerEvents.InvokeCurrentHealthEvent(currentHealth);
        playerEvents.InvokeMaxHealthEvent(playerData.maxHealth);
    }

    public virtual float GetCurrentHealth()
    {
        return currentHealth;
    }

    public virtual float GetMaxHealth()
    {
        return maxHealth;
    }

    // add or subtract from health count
    public virtual void ModifyHealth(float amount)
    {
        // if this health modification will exceed the max potential health, then just set the current health to max
        if (currentHealth + amount > playerData.maxHealth)
        {
            currentHealth = playerData.maxHealth;
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

    public virtual void ModifyMaxHealth(float amount)
    {
        maxHealth += amount;
        playerEvents.InvokeMaxHealthEvent(maxHealth);
        
    }

    // do something when this entity dies
    public bool IsDead()
    {
        return isDead;
    }

    public void UpdateScriptableObject(PlayerData scriptableObject)
    {
        playerData = scriptableObject;

        // to account for any items that affect the local maxHealth, and giving the player new data
        // For example, if we started off with 500 maxHealth, and retrieved an item that gave us an extra 250 health, we now have 750 maxHealth
        // But what if we decided to give the player a new set of health and movement speed (new PlayerData), we need to account for extra health from items
        // Likely won't happen since we might not have augments that will need to change PlayerData
        float difference = maxHealth - playerData.maxHealth;

        maxHealth = scriptableObject.maxHealth + difference;

    }
}
