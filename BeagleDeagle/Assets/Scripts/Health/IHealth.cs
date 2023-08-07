using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{

    ///-///////////////////////////////////////////////////////////
    /// Add or subtract from this entity's current health value
    /// 
    public void ModifyHealth(float amount);

    public void AddMaxHealthModifier(MaxHealthModifier modifierToAdd);

    public void RemoveMaxHealthModifier(MaxHealthModifier modifierToRemove);
    
    ///-///////////////////////////////////////////////////////////
    /// Return the current health value of this entity
    /// 
    public float GetCurrentHealth();

    ///-///////////////////////////////////////////////////////////
    /// Do something when this entity dies
    /// 
    public bool IsDead();
}
