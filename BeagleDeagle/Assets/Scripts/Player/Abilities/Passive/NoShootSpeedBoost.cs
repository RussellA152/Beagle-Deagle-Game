using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoShootSpeedBoost : PassiveAbility
{
    [SerializeField] private SpeedBoostData speedBoost;
    private bool _speedIncreased = false;
    
    private Gun _gunScript;

    private IMovable _movementScript;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        // Fetch gun script from the gun gameObject
        _gunScript = Player.GetComponentInChildren<Gun>();
        
        _movementScript = Player.GetComponent<IMovable>();
        
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
            if (!_speedIncreased && _gunScript.ReturnLastTimeShot() >= speedBoost.minimumTimeRequired)
            {
                _speedIncreased = true;

                _movementScript.AddMovementSpeedModifier(speedBoost.movementSpeedModifier);
           
            }
            else if (_speedIncreased && _gunScript.ReturnLastTimeShot() < speedBoost.minimumTimeRequired)
            {
                _speedIncreased = false;

                _movementScript.RemoveMovementSpeedModifier(speedBoost.movementSpeedModifier);
            }
            yield return null;
        }
    }

    protected override void RemovePassive()
    {
        IMovable movementScript = Player.GetComponent<IMovable>();

        movementScript.RemoveMovementSpeedModifier(speedBoost.movementSpeedModifier);
        
    }
}
