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

    public void AddDamageOverTime(DamageOverTime dotToAdd);

    public void RemoveDamageOverTime(DamageOverTime dotToRemove);

    public IEnumerator TakeDamageOverTime(DamageOverTime dot);

    // Do something when this entity dies
    public bool IsDead();
}
