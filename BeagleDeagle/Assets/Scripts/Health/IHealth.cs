using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float GetCurrentHealth();

    // add or subtract from health count
    public void ModifyHealth(float amount);

    public void AddMaxHealthModifier(MaxHealthModifier modifierToAdd);

    public void RemoveMaxHealthModifier(MaxHealthModifier modifierToRemove);

    // do something when this entity dies
    public bool IsDead();
}
