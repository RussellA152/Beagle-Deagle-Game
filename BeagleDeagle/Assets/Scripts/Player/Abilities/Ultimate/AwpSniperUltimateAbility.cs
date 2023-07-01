using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AwpSniperUltimateAbility : UltimateAbility<AwpSniperUltimateData>
{
    [SerializeField]
    private Gun playerGunScript;

    private GunData _previousWeaponData;
    
    private bool _isActive;

    private Coroutine _durationCoroutine;
    
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

        _isActive = true;

        playerGunScript.UpdateScriptableObject(ultimateData.awpGunData);
        
        // Don't allow player to reload when activating AWP
        playerGunScript.SetCanReload(false);

        _durationCoroutine = StartCoroutine(WeaponDuration());

    }
    
    public void CheckAmmoLoad(int ammoLoad)
    {
        // Only check ammo load when the gun that the player has is the AWP sniper
        if(_isActive &&  ammoLoad <= 0)
        {
            Debug.Log("AWP IS OUT OF AMMO!");
            
            StopCoroutine(_durationCoroutine);
            
            ReturnOriginalWeapon();

            StartCoroutine(UltimateCooldown());
            StartCoroutine(CountDownCooldown());
            
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
        if (_isActive)
        {
            Debug.Log("REMOVE AWP FROM PLAYER!");
            playerGunScript.UpdateScriptableObject(ultimateData.awpGunData);
            // Give back the player their gun before they received the AWP sniper
            playerEvents.InvokeNewWeaponEvent(_previousWeaponData);
            

            _isActive = false;
            
            // Allow the player to reload again
            playerGunScript.SetCanReload(true);

        }

    }

    private IEnumerator WeaponDuration()
    {
        if (_isActive)
        {
            yield return new WaitForSeconds(ultimateData.duration);
            ReturnOriginalWeapon();

            StartCoroutine(UltimateCooldown());
            StartCoroutine(CountDownCooldown());
            
        }

    }

}
    
    
