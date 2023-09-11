using System;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// A series of significant events caused by player actions. Some including when the player's health changes, obtains an upgrade to their gun,
/// uses their abilities, or shoots their weapon.
/// 
[CreateAssetMenu(menuName = "GameEvent/PlayerEvents")]
public class PlayerEvents : ScriptableObject
{
    // Pass a reference to the player gameObject
    public event Action<GameObject> givePlayerGameObject;
    
    // Pass a reference to the player's current Health (used by HUD)
    public event Action<float> onPlayerCurrentHealthChanged;

    // Pass a reference to the player's max Health (used by HUD)
    public event Action<float> onPlayerMaxHealthChanged;

    // Pass a reference to the player's current weapon data
    public event Action<GunData> onPlayerSwitchedWeapon;

    public event Action<int> onPlayerCurrentXpChanged;

    public event Action<int> onPlayerLeveledUp;

    // Pass a reference to the player's current stat data (might be used when the player receives new health and movement speed data?)
    public event Action<PlayerData> onPlayerObtainedNewCharacterStats;

    // Pass a reference to the player's current ammo loaded (invoked when the player's ammo changes)
    public event Action<int> onPlayerBulletsLoadedChanged;

    // Pass a reference to the player's current utility data (ex. player is currently using Mighty Foot)
    public event Action<UtilityAbilityData> onPlayerObtainedNewUtility;
    
    // Pass a reference to the player's utility uses (invoked when the player uses their utility ability)
    public event Action<int> onPlayerUtilityUsesUpdated;
    
    
    // Pass a reference to the player's current ultimate data (ex. player is currently using Nuke)
    public event Action<UltimateAbilityData> onPlayerObtainedNewUltimate; 
    
    // Give references to the player's specific cooldown IDs (these are needed by the Cooldown UI script)
    public event Action<int> givePlayerReloadCooldownId;
    public event Action<int> givePlayerRollCooldownId; 
    public event Action<int> giveUtilityCooldownId; 
    public event Action<int> giveUltimateCooldownId; 

    // Call this function when the player's ultimate ability concluded
    public event Action onPlayerUltimateEnded;

    ///-///////////////////////////////////////////////////////////
    /// In the PlayerController, pass a reference to the gameObject of the player. We use this instead
    /// of GameObject.FindObjectWithTag("Player")
    /// 
    public void InvokeFindPlayer(GameObject playerGameObject)
    {
        givePlayerGameObject?.Invoke(playerGameObject);
    }

    // When the player's max health changes
    // Pass around the max health value to whoever needs it (ex. HUD needs to display max health at all times)
    public void InvokeMaxHealthEvent(float maxHealth)
    {
        onPlayerMaxHealthChanged?.Invoke(maxHealth);
    }
    // When the player's current health changes
    // Pass around the current health value to whoever needs it (ex. HUD needs to display current health at all times)
    public void InvokeCurrentHealthEvent(float currentHealth)
    {
        onPlayerCurrentHealthChanged?.Invoke(currentHealth);
    }
    // When the player receives a new weapon or a modification to their current weapon
    // Pass around the data for the new weapon. It will allow things like the HUD to update the ammo count
    public void InvokeNewWeaponEvent(GunData newWeaponData)
    {
        onPlayerSwitchedWeapon?.Invoke(newWeaponData);
    }

    // When the player's xp amount changes, tell all listeners how much xp the player now currently has
    public void InvokeCurrentXpEvent(int amount)
    {
        onPlayerCurrentXpChanged?.Invoke(amount);
    }

    public void InvokePlayerLeveledUpEvent(int newLevel)
    {
        onPlayerLeveledUp?.Invoke(newLevel);
    }

    // When the player receives a new set of stats (New set of maxHealth, movementSpeed, etc. values)
    public void InvokeNewStatsEvent(PlayerData newPlayerData)
    {
        onPlayerObtainedNewCharacterStats?.Invoke(newPlayerData);
    }

    public void InvokeUpdateAmmoLoadedText(int ammoLoaded)
    {
        //Debug.Log("Ammo is: " + ammoLoaded);
        onPlayerBulletsLoadedChanged?.Invoke(ammoLoaded);
    }
    public void InvokeReloadCooldown(int id)
    {
        givePlayerReloadCooldownId?.Invoke(id);
    }
    
    public void InvokeRollCooldown(int id)
    {
        givePlayerRollCooldownId?.Invoke(id);
    }
    public void InvokeUtilityCooldown(int id)
    {
        giveUtilityCooldownId?.Invoke(id);
    }
    public void InvokeUltimateCooldown(int id)
    {
        giveUltimateCooldownId?.Invoke(id);
    }

    public void InvokeNewUtility(UtilityAbilityData newUtilityData)
    {
        onPlayerObtainedNewUtility?.Invoke(newUtilityData);
    }
    
    public void InvokeNewUltimate(UltimateAbilityData newUltimateData)
    {
        onPlayerObtainedNewUltimate?.Invoke(newUltimateData);
    }
    // When the player uses a utility ability, invoke this function.
    // This should pass around the current number of uses that the player's currently utility has (ex. HUD needs to update utility uses display)
    public void InvokeUtilityUsesUpdatedEvent(int uses)
    {
        onPlayerUtilityUsesUpdated?.Invoke(uses);
    }
    
    public void InvokeUltimateAbilityEnded()
    {
        onPlayerUltimateEnded?.Invoke();
    }

}
