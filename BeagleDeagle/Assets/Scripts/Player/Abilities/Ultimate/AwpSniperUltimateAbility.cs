using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AwpSniperUltimateAbility : UltimateAbility<AwpSniperUltimateData>
{
    private IGunDataUpdatable playerGunScript;

    private GunData _previousWeaponData;
    
    protected bool isActive;

    private Coroutine durationCoroutine;

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

        durationCoroutine = StartCoroutine(Duration());

    }
    
    public void CheckAmmoLoad(int ammoLoad)
    {
        // Only check ammo load when the gun that the player has is the AWP sniper
        if(isActive &&  ammoLoad <= 0)
        {
            Debug.Log("AWP IS OUT OF AMMO!");
            
            StopCoroutine(durationCoroutine);
            
            ReturnOriginalWeapon();

            StartCoroutine(Cooldown());
            StartCoroutine(CountDownCooldown());
            
            
        
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

    private IEnumerator Duration()
    {
        if (isActive)
        {
            yield return new WaitForSeconds(ultimateData.duration);
            ReturnOriginalWeapon();

            StartCoroutine(Cooldown());
            StartCoroutine(CountDownCooldown());


        }

    }


}
    
    
