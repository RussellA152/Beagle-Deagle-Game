using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public event Action onDeath;
    
    public event Action onTookDamage;
    
    ///-///////////////////////////////////////////////////////////
    /// Add or subtract from this entity's current health value
    /// 
    public void ModifyHealth(float amount);

    ///-///////////////////////////////////////////////////////////
    /// Return the current health value of this entity
    /// 
    public float GetCurrentHealth();

    public bool IsHealthBelowPercentage(float healthPercentage);

    ///-///////////////////////////////////////////////////////////
    /// Do something when this entity dies
    /// 
    public void InvokeDeathEvent();

    public void InvokeTookDamageEvent();

    public bool IsDead();
}
