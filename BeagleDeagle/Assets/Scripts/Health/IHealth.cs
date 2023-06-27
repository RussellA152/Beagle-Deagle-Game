using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float GetCurrentHealth();

    // Add or subtract from health count
    public void ModifyHealth(float amount);

    public void AddMaxHealthModifier(MaxHealthModifier modifierToAdd);

    public void RemoveMaxHealthModifier(MaxHealthModifier modifierToRemove);

    // Do something when this entity dies
    public bool IsDead();
}
