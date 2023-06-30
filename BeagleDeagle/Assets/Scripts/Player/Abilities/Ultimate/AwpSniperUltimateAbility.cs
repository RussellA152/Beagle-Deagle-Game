using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AwpSniperUltimateAbility : UltimateAbility<AwpSniperUltimateData>
{
    //private bool _firstTimeUse;

    private IGunDataUpdatable playerGunScript;

    private GunData _previousWeaponData;
    
    protected bool isActive;

    protected override void Start()
    {
        base.Start();

        playerGunScript = gameObject.GetComponentInChildren<IGunDataUpdatable>();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        
        playerEvents.onPlayerSwitchedWeapon += UpdateCurrentWeapon;

        playerEvents.onPlayerBulletsLoadedChanged += CheckAmmoLoad;

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        playerEvents.onPlayerBulletsLoadedChanged -= CheckAmmoLoad;

        playerEvents.onPlayerSwitchedWeapon -= UpdateCurrentWeapon;
    }
    protected override void UltimateAction(GameObject player)
    {
        Debug.Log("Give player an awp!");

        isActive = true;

        playerGunScript.UpdateScriptableObject(ultimateData.awpGunData);

        StartCoroutine(WaitForUse());
        //StartCoroutine(CountDownCooldown());

    }
    
    public void CheckAmmoLoad(int ammoLoad)
    {
        // Only check ammo load when the gun that the player has is the AWP sniper
        if(isActive &&  ammoLoad<= 0)
        {
            Debug.Log("AWP IS OUT OF AMMO!");
            
            ReturnOriginalWeapon();
        
            playerEvents.InvokeNewWeaponEvent(_previousWeaponData);
        }
    }
    
    // Save a reference to the gun the player had before receiving AWP sniper
    public void UpdateCurrentWeapon(GunData gunData)
    {
        // Check the current weapon that the player has
        // Don't update this value if the player received an AWP sniper
        if(gunData != ultimateData.awpGunData)
        {
            _previousWeaponData = gunData;
        }
    }

    public void ReturnOriginalWeapon()
    {
        // Only give back original weapon if the player has the AWP
        if (isActive)
        {
            Debug.Log("REMOVE AWP FROM PLAYER!");
            // Give back the player their gun before they received the AWP sniper
            playerEvents.InvokeNewWeaponEvent(_previousWeaponData);
            

            isActive = false;

        }

    }

    protected override IEnumerator WaitForUse()
    {
        canUseUltimate = false;
        
        // If the player currently has the AWP sniper, then wait until the duration ends to give back their original weapon
        if (isActive)
        {
            yield return new WaitForSeconds(ultimateData.duration);

            ReturnOriginalWeapon();
            
        }
        Debug.Log("Start waiting for awp's cooldown!                 1");
        
        StartCoroutine(CountDownCooldown());
        
        yield return new WaitForSeconds(ultimateData.cooldown);

        canUseUltimate = true;
        Debug.Log("AWP ULTIMATE READY!");

    }

    protected override IEnumerator CountDownCooldown()
    {
        // Wait until ultimate is no longer active to begin cooldown
        // while (isActive)
        //     yield return null;
    
        // Call the base implementation of the method
        yield return base.CountDownCooldown();
    }

}
    
    
