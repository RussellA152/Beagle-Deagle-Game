using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A series of significant events caused by player actions. Some including when the player's health changes, obtains an upgrade to their gun,
/// uses their abilities, or shoots their weapon.
/// </summary>
[CreateAssetMenu(menuName = "GameEvent/PlayerEvents")]
public class PlayerEvents : ScriptableObject
{
    public event Action<PlayerInput> onPlayerInputInitialization; // Pass a reference to the player's PlayerInput component (could be useful for gameObjects that need reference, but not attached to player)

    public event Action<float> onPlayerCurrentHealthChanged; // Pass a reference to the player's current Health (used by HUD)

    public event Action<float> onPlayerMaxHealthChanged; // Pass a reference to the player's max Health (used by HUD)

    public event Action<GunData> onPlayerSwitchedWeapon; // Pass a reference to the player's current weapon data
    
    public event Action<CharacterData> onPlayerObtainedNewCharacterStats; // Pass a reference to the player's current stat data (might be used when the player receives new health and movement speed data?)

    public event Action<int> onPlayerBulletsLoadedChanged; // Pass a reference to the player's current ammo loaded (invoked when the player's ammo changes)

    public event Action<int> onPlayerUtilityUsesUpdated; // Pass a reference to the player's utility uses (invoked when the player uses their utility ability)

    public event Action<string> onPlayerUtilityNameChanged; // Pass a reference to the name of the player's utility ability (invoked when the player obtains a new utility ability. Mainly for debugging)

    public event Action<float> onPlayerUltimateAbilityCooldown; // Pass a reference to how much time is left on the player's ultimate ability cooldown

    public event Action<string> onPlayerUltimateAbilityNameChanged; // Pass a reference to the name of the player's ultimate ability (invoked when the player obtains a new ultimate ability. Mainly for debugging)

    // Call this function when the player's ultimate ability concluded
    public event Action onPlayerUltimateEnded;  

    // When the player's max health changes
    // Pass around the max health value to whoever needs it (ex. HUD needs to display max health at all times)
    public void InvokeMaxHealthEvent(float maxHealth)
    {
        if(onPlayerMaxHealthChanged != null)
        {
            onPlayerMaxHealthChanged(maxHealth);
        }
    }
    // When the player's current health changes
    // Pass around the current health value to whoever needs it (ex. HUD needs to display current health at all times)
    public void InvokeCurrentHealthEvent(float currentHealth)
    {
        if (onPlayerCurrentHealthChanged != null)
        {
            onPlayerCurrentHealthChanged(currentHealth);
        }
    }
    // When the player receives a new weapon or a modification to their current weapon
    // Pass around the data for the new weapon. It will allow things like the HUD to update the ammo count
    public void InvokeNewWeaponEvent(GunData newWeaponData)
    {
        if(onPlayerSwitchedWeapon != null)
        {
            onPlayerSwitchedWeapon(newWeaponData);
        }
    }

    // When the player receives a new set of stats (New set of maxHealth, movementSpeed, etc. values)
    public void InvokeNewStatsEvent(CharacterData newPlayerData)
    {
        if (onPlayerObtainedNewCharacterStats != null)
        {
            onPlayerObtainedNewCharacterStats(newPlayerData);
        }
    }

    public void InvokeUpdateAmmoLoadedText(int ammoLoaded)
    {
        if(onPlayerBulletsLoadedChanged != null)
        {
            //Debug.Log("Ammo is: " + ammoLoaded);
            onPlayerBulletsLoadedChanged(ammoLoaded);
        }
    }

    // When the player uses a utility ability, invoke this function.
    // This should Pass around the current number of uses that the player's currently utility has (ex. HUD needs to update utility uses display)
    public void InvokeUtilityUsesUpdatedEvent(int uses)
    {
        if(onPlayerUtilityUsesUpdated != null)
        {
            onPlayerUtilityUsesUpdated(uses);
        }
    }
    public void InvokeUtilityNameUpdatedEvent(string name)
    {
        if (onPlayerUtilityNameChanged != null)
        {
            onPlayerUtilityNameChanged(name);
        }
    }

    public void InvokeGivePlayerInputComponentEvent(PlayerInput inputComponent)
    {
        if (onPlayerInputInitialization != null)
        {
            onPlayerInputInitialization(inputComponent);
        }
    }

    public void InvokeUltimateAbilityEnded()
    {
        if (onPlayerUltimateEnded != null)
        {
            onPlayerUltimateEnded();
        }
    }

    public void InvokeUltimateAbilityCooldownEvent(float timeLeft)
    {
        if(onPlayerUltimateAbilityCooldown != null)
        {
            onPlayerUltimateAbilityCooldown(timeLeft);
        }
    }

    public void InvokeUltimateNameUpdatedEvent(string name)
    {
        if(onPlayerUltimateAbilityNameChanged != null)
        {
            onPlayerUltimateAbilityNameChanged(name);
        }
    }

}
