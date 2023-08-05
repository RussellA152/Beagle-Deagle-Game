using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CooldownData
{
    public int Id { get;  }
    public float RemainingTime { get; private set; }
    

    ///-///////////////////////////////////////////////////////////
    /// Cooldown data constructor
    /// Will be used to create a cooldown with a unique id
    public CooldownData(IHasCooldown cooldown)
    {
        Id = cooldown.Id;
        RemainingTime = cooldown.CooldownDuration;
    }

    ///-///////////////////////////////////////////////////////////
    /// Will be called to decrease the time of the cooldown, returns true when cooldown is over
    /// 
    public bool DecrementCooldown(float deltaTime)
    {
        //Stop time from going negative
        RemainingTime = Mathf.Max(RemainingTime - deltaTime, 0f);

        return RemainingTime == 0f;
    }
}
