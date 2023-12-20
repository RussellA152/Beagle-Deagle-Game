using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUtilityUpdatable: IDataUpdatable<UtilityAbilityData>
{
    ///-///////////////////////////////////////////////////////////
    /// Allow or disable the use of the player's utility ability
    /// 
    public void AllowUtility(bool boolean);

    public void AddUtilityCooldownModifier(UtilityCooldownModifier modifierToAdd);

    public void RemoveUtilityCooldownModifier(UtilityCooldownModifier modifierToRemove);

    public void AddUtilityUsesModifier(UtilityUsesModifier modifierToAdd);
    public void RemoveUtilityUsesModifier(UtilityUsesModifier modifierToRemove);
}
