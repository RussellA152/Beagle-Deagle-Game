using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "GameEvent/PlayerEvents")]
public class PlayerEventSO : ScriptableObject
{
    public event Action<IPlayerStatModifier> givePlayerStatModifierScriptEvent;

    public event Action<PlayerInput> givePlayerInputComponentEvent;

    public event Action<float> currentHealthChangedEvent;

    public event Action<float> maxHealthChangedEvent;

    public event Action<GunData> playerObtainedNewWeaponEvent;

    public event Action<PlayerData> playerObtainedNewStatsEvent;

    public event Action<int> playerAmmoHUDUpdateEvent;

    public event Action<int> playerUtilityUsesUpdatedEvent;

    public event Action<string> playerUtilityNameChangeEvent;

    // When the player's max health changes
    // Pass around the max health value to whoever needs it (ex. HUD needs to display max health at all times)
    public void InvokeMaxHealthEvent(float maxHealth)
    {
        if(maxHealthChangedEvent != null)
        {
            maxHealthChangedEvent(maxHealth);
        }
    }
    // When the player's current health changes
    // Pass around the current health value to whoever needs it (ex. HUD needs to display current health at all times)
    public void InvokeCurrentHealthEvent(float currentHealth)
    {
        if (currentHealthChangedEvent != null)
        {
            currentHealthChangedEvent(currentHealth);
        }
    }
    // When the player receives a new weapon or a modification to their current weapon
    // Pass around the data for the new weapon. It will allow things like the HUD to update the ammo count
    public void InvokeNewWeaponEvent(GunData newWeaponData)
    {
        if(playerObtainedNewWeaponEvent != null)
        {
            playerObtainedNewWeaponEvent(newWeaponData);
        }
    }

    // When the player receives a new set of stats (New set of maxHealth, movementSpeed, etc. values)
    public void InvokeNewStatsEvent(PlayerData newPlayerData)
    {
        if (playerObtainedNewStatsEvent != null)
        {
            playerObtainedNewStatsEvent(newPlayerData);
        }
    }

    public void InvokeUpdateAmmoLoadedText(int ammoLoaded)
    {
        if(playerAmmoHUDUpdateEvent != null)
        {
            playerAmmoHUDUpdateEvent(ammoLoaded);
        }
    }

    // When the player uses a utility ability, invoke this function.
    // This should pass around the current number of uses that the player's currently utility has (ex. HUD needs to update utility uses display)
    public void InvokeUtilityUsesUpdatedEvent(int uses)
    {
        if(playerUtilityUsesUpdatedEvent != null)
        {
            playerUtilityUsesUpdatedEvent(uses);
        }
    }
    public void InvokeUtilityNameUpdatedEvent(string name)
    {
        if (playerUtilityNameChangeEvent != null)
        {
            playerUtilityNameChangeEvent(name);
        }
    }

    public void InvokeGivePlayerStatModifierScript(IPlayerStatModifier modifierScript)
    {
        if(givePlayerStatModifierScriptEvent != null)
        {
            givePlayerStatModifierScriptEvent(modifierScript);
        }
    }

    public void InvokeGivePlayerInputComponentEvent(PlayerInput inputComponent)
    {
        if(givePlayerInputComponentEvent != null)
        {
            givePlayerInputComponentEvent(inputComponent);
        }
    }

}
