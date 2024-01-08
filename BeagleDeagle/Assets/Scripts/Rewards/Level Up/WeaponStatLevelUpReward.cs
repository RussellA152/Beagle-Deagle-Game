using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class WeaponStatLevelUpReward: LevelUpReward
{
    public WeaponStatModifierData weaponStatModifierData;
    
    public WeaponStatLevelUpReward(WeaponStatModifierData data, int level)
    {
        weaponStatModifierData = data;
        LevelGiven = level;
    }
    public override string GetRewardName()
    {
        return "Weapon Stat Modifier";
    }
    
    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        ModifierManager modifierManager = recipientGameObject.GetComponent<ModifierManager>();

        if(weaponStatModifierData.DamageModifier.IsModifierNameValid())
            modifierManager.AddModifier(weaponStatModifierData.DamageModifier);
        
        if(weaponStatModifierData.PenetrationModifier.IsModifierNameValid())
            modifierManager.AddModifier(weaponStatModifierData.PenetrationModifier);

        if (weaponStatModifierData.SpreadModifier.IsModifierNameValid())
            modifierManager.AddModifier(weaponStatModifierData.SpreadModifier);
        
        if(weaponStatModifierData.ReloadSpeedModifier.IsModifierNameValid())
            modifierManager.AddModifier(weaponStatModifierData.ReloadSpeedModifier);
        
        if(weaponStatModifierData.AttackSpeedModifier.IsModifierNameValid())
            modifierManager.AddModifier(weaponStatModifierData.ReloadSpeedModifier);
        
        if(weaponStatModifierData.AmmoLoadModifier.IsModifierNameValid())
            modifierManager.AddModifier(weaponStatModifierData.AmmoLoadModifier);
        
        Debug.Log($"{recipientGameObject.name} was given {weaponStatModifierData}");
    }
    
    public override string GetDescription()
    {
        return weaponStatModifierData.GetDescription();
    }
}