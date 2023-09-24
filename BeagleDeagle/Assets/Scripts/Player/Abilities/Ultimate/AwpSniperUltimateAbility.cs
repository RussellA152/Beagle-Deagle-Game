using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwpSniperUltimateAbility : UltimateAbility<AwpSniperUltimateData>
{
    private Gun _playerGunScript;
    
    [SerializeField] private GunData _previousWeaponData;
    
    private bool _isActive;

    private Coroutine _durationCoroutine;

    private float _returnOriginalWeaponDelay = 0.5f;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        _playerGunScript = GetComponentInChildren<Gun>();


        playerEvents.getAllWeaponDataUpdates += UpdateCurrentWeapon;

        playerEvents.onPlayerBulletsLoadedChanged += CheckAmmoLoad;

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playerEvents.onPlayerBulletsLoadedChanged -= CheckAmmoLoad;
        playerEvents.getAllWeaponDataUpdates -= UpdateCurrentWeapon;
    }
    
    protected override void UltimateAction()
    {
        // Give the player an AWP Sniper as their new gun
        _isActive = true;

        _playerGunScript.UpdateScriptableObject(ultimateData.awpGunData);

        _playerGunScript.AllowWeaponReceive(false);
        // Don't allow player to reload when activating AWP
        _playerGunScript.AllowReload(false);

        _durationCoroutine = StartCoroutine(WeaponDuration());

    }
    
    private void CheckAmmoLoad(int ammoLoad)
    {
        // Only check ammo load when the gun that the player has is the AWP sniper
        if(_isActive &&  ammoLoad <= 0)
        {
            StopCoroutine(_durationCoroutine);
            
            StartCoroutine(ReturnOriginalWeapon());
            //ReturnOriginalWeapon();

        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Ultimate ability will remember what weapon the player had before swapping to the AWP sniper.
    /// If the player's weapon gets updated while ulting, they will switch back to that new updated weapon.
    /// 
    private void UpdateCurrentWeapon(List<GunData> allPreviousWeapons)
    {
        GunData mostRecentWeapon = allPreviousWeapons[allPreviousWeapons.Count - 1];
        // Check the current weapon that the player has
        // Don't update this value if the player received an AWP sniper
        if(mostRecentWeapon != ultimateData.awpGunData)
        {
            _previousWeaponData = mostRecentWeapon;
        }
    }

    private IEnumerator ReturnOriginalWeapon()
    {
        // Only give back original weapon if the player has the AWP
        if (_isActive)
        {
            // Wait a little before giving the player their original weapon back
            // * Without this, the ammo display will show 0 current ammo because we shot the weapon and tried to give a new gun to the player *
            yield return new WaitForSeconds(_returnOriginalWeaponDelay);
            
            // Give back the player their gun before they received the AWP sniper
            _playerGunScript.AllowWeaponReceive(true);
            _playerGunScript.UpdateScriptableObject(_previousWeaponData);
            
            
            // Activate cooldown
            StartCooldown();

            _isActive = false;
            
            // Allow the player to reload again
            _playerGunScript.AllowReload(true);

        }
    }

    ///-///////////////////////////////////////////////////////////
    /// If the duration of the awp runs out, then
    /// return the player's original weapon.
    /// 
    private IEnumerator WeaponDuration()
    {
        if (_isActive)
        {
            yield return new WaitForSeconds(ultimateData.duration);
            StartCoroutine(ReturnOriginalWeapon());

        }

    }

}
    
    
