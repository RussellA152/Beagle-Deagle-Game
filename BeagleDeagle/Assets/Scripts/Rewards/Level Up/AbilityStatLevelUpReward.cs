using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class AbilityStatLevelUpReward : LevelUpReward
{
    public AbilityStatModifierData abilityStatModifierData;
    
    public AbilityStatLevelUpReward(AbilityStatModifierData data, int level)
    {
        abilityStatModifierData = data;
        
        LevelGiven = level;
    }
    public override string GetRewardName()
    {
        return "Ability Stat Upgrade";
    }
    
    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        ModifierManager modifierManager = recipientGameObject.GetComponent<ModifierManager>();
        
        if(abilityStatModifierData.utilityDamageModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.utilityDamageModifier);
        
        if(abilityStatModifierData.UtilityUsesModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.UtilityUsesModifier);
        
        if(abilityStatModifierData.UtilityCooldownModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.UtilityCooldownModifier);
        
        if(abilityStatModifierData.ultimateDamageModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.ultimateDamageModifier);
        
        if(abilityStatModifierData.UltimateCooldownModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.UltimateCooldownModifier);
        
        
        Debug.Log($"{recipientGameObject.name} was given {abilityStatModifierData}");
    }
    
    public override string GetDescription()
    {
        return abilityStatModifierData.GetDescription();
    }
}
