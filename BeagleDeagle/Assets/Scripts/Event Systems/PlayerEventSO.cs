using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A series of significant events caused by player actions. Some including when the player's health changes, obtains an upgrade to their gun,
/// uses their abilities, or shoots their weapon.
/// </summary>
[CreateAssetMenu(menuName = "GameEvent/PlayerEvents")]
public class PlayerEventSO : ScriptableObject
{
    public event Action<PlayerInput> givePlayerInputComponentEvent; // Pass a reference to the player's PlayerInput component (could be useful for gameObjects that need reference, but not attached to player)

    public event Action<float> currentHealthChangedEvent; // Pass a reference to the player's current Health (used by HUD)

    public event Action<float> maxHealthChangedEvent; // Pass a reference to the player's max Health (used by HUD)

    public event Action<GunData> playerObtainedNewWeaponEvent; // Pass a reference to the player's current weapon data

    public event Action<PlayerData> playerObtainedNewStatsEvent; // Pass a reference to the player's current stat data (might be used when the player receives new health and movement speed data?)

    public event Action<int> playerBulletsLoadedChangedEvent; // Pass a reference to the player's current ammo loaded (invoked when the player's ammo changes)

    public event Action<int> playerUtilityUsesUpdatedEvent; // Pass a reference to the player's utility uses (invoked when the player uses their utility ability)

    public event Action<string> playerUtilityNameChangeEvent; // Pass a reference to the name of the player's utility ability (invoked when the player obtains a new utility ability. Mainly for debugging)

    public event Action<float> playerUltimateCooldownEvent; // Pass a reference to how much time is left on the player's ultimate ability cooldown

    public event Action<string> playerUltimateNameChangeEvent; // Pass a reference to the name of the player's ultimate ability (invoked when the player obtains a new ultimate ability. Mainly for debugging)

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
        if(playerBulletsLoadedChangedEvent != null)
        {
            //Debug.Log("Ammo is: " + ammoLoaded);
            playerBulletsLoadedChangedEvent(ammoLoaded);
        }
    }

    // When the player uses a utility ability, invoke this function.
    // This should Pass around the current number of uses that the player's currently utility has (ex. HUD needs to update utility uses display)
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

    public void InvokeGivePlayerInputComponentEvent(PlayerInput inputComponent)
    {
        if (givePlayerInputComponentEvent != null)
        {
            givePlayerInputComponentEvent(inputComponent);
        }
    }

    public void InvokeUltimateAbilityCooldownEvent(float timeLeft)
    {
        if(playerUltimateCooldownEvent != null)
        {
            playerUltimateCooldownEvent(timeLeft);
        }
    }

    public void InvokeUltimateNameUpdatedEvent(string name)
    {
        if(playerUltimateNameChangeEvent != null)
        {
            playerUltimateNameChangeEvent(name);
        }
    }

}
