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
        return "Weapon Stat Upgrade";
    }
    
    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        IGunDataUpdatable gunScript = recipientGameObject.GetComponentInChildren<IGunDataUpdatable>();
        
        if(weaponStatModifierData.DamageModifier.IsModifierNameValid())
            gunScript.AddDamageModifier(weaponStatModifierData.DamageModifier);
        
        if(weaponStatModifierData.PenetrationModifier.IsModifierNameValid())
            gunScript.AddPenetrationModifier(weaponStatModifierData.PenetrationModifier);

        if (weaponStatModifierData.SpreadModifier.IsModifierNameValid())
            gunScript.AddSpreadModifier(weaponStatModifierData.SpreadModifier);
        
        if(weaponStatModifierData.ReloadSpeedModifier.IsModifierNameValid())
            gunScript.AddReloadSpeedModifier(weaponStatModifierData.ReloadSpeedModifier);
        
        if(weaponStatModifierData.AttackSpeedModifier.IsModifierNameValid())
            gunScript.AddReloadSpeedModifier(weaponStatModifierData.ReloadSpeedModifier);
        
        if(weaponStatModifierData.AmmoLoadModifier.IsModifierNameValid())
            gunScript.AddAmmoLoadModifier(weaponStatModifierData.AmmoLoadModifier);
        
        Debug.Log($"{recipientGameObject.name} was given {weaponStatModifierData}");
    }
    
    public override string GetDescription()
    {
        return weaponStatModifierData.GetDescription();
    }
}