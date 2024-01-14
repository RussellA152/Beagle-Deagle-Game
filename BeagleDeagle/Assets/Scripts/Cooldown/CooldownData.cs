using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CooldownData
{
    public int Id { get;  }

    public float startingTime { get; private set; }
    public float RemainingTime { get; private set; }
    

    ///-///////////////////////////////////////////////////////////
    /// Cooldown data constructor
    /// Will be used to create a cooldown with a unique id
    public CooldownData(IHasCooldown cooldown)
    {
        Id = cooldown.Id;
        
        startingTime = cooldown.CooldownDuration;
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

    public void RefreshCooldownTime()
    {
        RemainingTime = startingTime;
    }

    public void EndTimer()
    {
        RemainingTime = 0f;
    }

    public void ChangeRemainingTime(float newTime)
    {
        if (newTime > 0)
        {
            RemainingTime = newTime;
        }
            
        else
        {
            Debug.Log("New timer for cooldown data was invalid!");
        }
    }
}
