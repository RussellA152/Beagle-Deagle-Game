using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUltimateUpdatable: IDataUpdatable<UltimateAbilityData>
{
    ///-///////////////////////////////////////////////////////////
    /// Allow or disable the use of the player's ultimate ability
    /// 
    public void AllowUltimate(bool boolean);
    
    public void AddUltimateCooldownModifier(UltimateCooldownModifier modifierToAdd);

    public void RemoveUltimateCooldownModifier(UltimateCooldownModifier modifierToRemove);
    
    public void AddUltimateDamageModifier(UltimateDamageModifier modifierToAdd);

    public void RemoveUltimateDamageModifier(UltimateDamageModifier modifierToRemove);

    
}
