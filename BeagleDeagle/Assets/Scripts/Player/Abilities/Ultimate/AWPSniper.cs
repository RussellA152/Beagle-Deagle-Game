using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAWPUltimate", menuName = "ScriptableObjects/Ability/Ultimate/AWP Sniper")]
public class AWPSniper : UltimateAbilityData
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    [SerializeField]
    private AWPGunData awpGunData; 

    private GunData previousWeaponData;


    private void OnEnable()
    {
        previousWeaponData = null;

        playerEvents.playerObtainedNewWeaponEvent += UpdateCurrentWeapon;

        playerEvents.playerBulletsLoadedChangedEvent += CheckAmmoLoad;

    }

    private void OnDisable()
    {
        playerEvents.playerBulletsLoadedChangedEvent -= CheckAmmoLoad;

        playerEvents.playerObtainedNewWeaponEvent -= UpdateCurrentWeapon;
    }

    public override void ActivateUltimate(GameObject player)
    {
        Debug.Log("Give player an awp!");

        isActive = true;

        player.GetComponentInChildren<IGunDataUpdatable>().UpdateScriptableObject(awpGunData);

    }

    public void CheckAmmoLoad(int ammoLoad)
    {
        // Only check ammo load when the gun that the player has is the AWP sniper
        if(isActive && ammoLoad <= 0)
        {
            Debug.Log("AWP IS OUT OF AMMO!");

            ReturnOriginalWeapon();
        }
    }

    // Save a reference to the gun the player had before receiving AWP sniper
    public void UpdateCurrentWeapon(GunData gunData)
    {
        // Check the current weapon that the player has
        // Don't update this value if the player received an AWP sniper
        if(gunData != awpGunData)
        {
            previousWeaponData = gunData;
        }
    }

    public void ReturnOriginalWeapon()
    {
        // Only give back original weapon if the player has the AWP
        if (isActive)
        {
            Debug.Log("REMOVE AWP FROM PLAYER!");
            // Give back the player their gun before they received the AWP sniper
            playerEvents.InvokeNewWeaponEvent(previousWeaponData);

            isActive = false;

        }
        
    }

    public override IEnumerator ActivationCooldown()
    {
        // If the player currently has the AWP sniper, then wait until the duration ends to give back their original weapon
        if (isActive)
        {
            yield return new WaitForSeconds(duration);

            ReturnOriginalWeapon();

            // Begin cooldown after duration ends
            yield return new WaitForSeconds(cooldown);
        }

        else
        {
            // Begin cooldown whether or not the player has the AWP sniper or not
            yield return new WaitForSeconds(cooldown);
        }     

        Debug.Log("AWP ULTIMATE READY!");

    }
}
