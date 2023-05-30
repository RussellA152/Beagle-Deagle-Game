using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamager 
{
    public void AddDamageModifier(DamageModifier modifierToAdd);

    public void RemoveDamageModifier(DamageModifier modifierToRemove);

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd);

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove);
}
