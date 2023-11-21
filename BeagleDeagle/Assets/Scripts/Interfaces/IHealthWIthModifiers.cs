using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthWIthModifiers
{
    public void AddMaxHealthModifier(MaxHealthModifier modifierToAdd);

    public void RemoveMaxHealthModifier(MaxHealthModifier modifierToRemove);
}
