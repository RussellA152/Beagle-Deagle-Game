using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NoShootSpeedBoost : PassiveAbility
{
    [SerializeField] private MovementSpeedBoostData movementSpeedBoost;

    private ModifierManager _modifierManager;
    
    // How long must the player not attack for, to receive a speed boost?
    [Range(0f, 10f)]
    public float minimumTimeRequired;
    
    private bool _speedIncreased = false;
    
    private GunShooting _gunShooting;

    protected override void OnEnable()
    {
        // Fetch gun script from the gun gameObject
        _gunShooting = Player.GetComponentInChildren<GunShooting>();
        
        _modifierManager = Player.GetComponent<ModifierManager>();
        
        ShowOnBuffBar.SetBuffModifier(movementSpeedBoost.movementSpeedModifier);
        ShowOnBuffBar.SetModifierManager(_modifierManager);

        base.OnEnable();
        
    }

    protected override void OnDisable()
    {
        StopAllCoroutines();
        
        base.OnDisable();
    }

    protected override void ActivatePassive()
    {
        StartCoroutine(CheckIfPlayerShot());
    }

    ///-///////////////////////////////////////////////////////////
    /// Always check if the player has recently shot their weapon,
    /// if not, then add a speed boost.
    /// Otherwise remove the speed boost (if active)
    /// 
    private IEnumerator CheckIfPlayerShot()
    {
        while (true)
        {
            if (!_speedIncreased && _gunShooting.ReturnLastTimeShot() >= minimumTimeRequired)
            {
                _speedIncreased = true;

                _modifierManager.AddModifier(movementSpeedBoost.movementSpeedModifier);
                
                ShowOnBuffBar.ShowBuffIconWithoutDuration();
                
           
            }
            else if (_speedIncreased && _gunShooting.ReturnLastTimeShot() < minimumTimeRequired)
            {
                _speedIncreased = false;

                _modifierManager.RemoveModifier(movementSpeedBoost.movementSpeedModifier);
                
                ShowOnBuffBar.RemoveIconFromBuffBar();
                
            }
            yield return null;
        }
        
    }

    protected override void RemovePassive()
    {
        _modifierManager.RemoveModifier(movementSpeedBoost.movementSpeedModifier);
        
    }
}
