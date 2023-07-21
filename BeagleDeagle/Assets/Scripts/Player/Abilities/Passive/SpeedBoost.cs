using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PassiveAbility<SpeedBoostData>
{
    private bool _speedIncreased = false;
    
    private Gun _gunScript;

    private IMovable _movementScript;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        _gunScript = player.GetComponentInChildren<Gun>();
        
        _movementScript = player.GetComponent<IMovable>();
        
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
            //Debug.Log("while loop!");
            if (!_speedIncreased && _gunScript.ReturnLastTimeShot() >= passiveData.minimumTimeRequired)
            {
                _speedIncreased = true;

                _movementScript.AddMovementSpeedModifier(passiveData.movementSpeedModifier);
           
            }
            // Otherwise, if the player did receive a speed increase, and they started shooting...
            // Then revert speed back to what it was
            else if (_speedIncreased && _gunScript.ReturnLastTimeShot() < passiveData.minimumTimeRequired)
            {
                _speedIncreased = false;

                _movementScript.RemoveMovementSpeedModifier(passiveData.movementSpeedModifier);
            }
            
            yield return null;
        }
    }

    protected override void RemovePassive()
    {
        IMovable movementScript = player.GetComponent<IMovable>();

        movementScript.RemoveMovementSpeedModifier(passiveData.movementSpeedModifier);
        
    }
}
