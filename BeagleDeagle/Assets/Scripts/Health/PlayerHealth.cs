using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth, IPlayerDataUpdatable
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    private IPlayerStatModifier playerStatModifierScript;

    private float currentHealth;
    //private float maxHealth; // we make a local variable in here so that we don't affect original SO data
    private bool isDead;

    [SerializeField]
    private PlayerData playerData;

    private void Awake()
    {
        playerEvents.givePlayerStatModifierScriptEvent += UpdatePlayerStatsModifierScript;
    }

    private void OnDestroy()
    {
        playerEvents.givePlayerStatModifierScriptEvent -= UpdatePlayerStatsModifierScript;
    }

    private void Start()
    {
        InitializeHealth();
    }

    public void InitializeHealth()
    {
        isDead = false;

        currentHealth = playerData.maxHealth * playerStatModifierScript.GetMaxHealthModifier();

        playerEvents.InvokeCurrentHealthEvent(currentHealth);
        playerEvents.InvokeMaxHealthEvent(playerData.maxHealth * playerStatModifierScript.GetMaxHealthModifier());
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
        if (currentHealth + amount > playerData.maxHealth * playerStatModifierScript.GetMaxHealthModifier())
        {
            currentHealth = playerData.maxHealth * playerStatModifierScript.GetMaxHealthModifier();
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

    // do something when this entity dies
    public bool IsDead()
    {
        return isDead;
    }

    public void UpdateScriptableObject(PlayerData scriptableObject)
    {
        playerData = scriptableObject;

    }

    public void UpdatePlayerStatsModifierScript(IPlayerStatModifier modifierScript)
    {
        playerStatModifierScript = modifierScript;
    }
}
