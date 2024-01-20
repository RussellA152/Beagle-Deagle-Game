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
        
        if(abilityStatModifierData.utilityUsesModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.utilityUsesModifier);
        
        if(abilityStatModifierData.utilityCooldownModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.utilityCooldownModifier);
        
        if(abilityStatModifierData.ultimateDamageModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.ultimateDamageModifier);
        
        if(abilityStatModifierData.ultimateCooldownModifier.IsModifierNameValid())
            modifierManager.AddModifier(abilityStatModifierData.ultimateCooldownModifier);
        
        
        Debug.Log($"{recipientGameObject.name} was given {abilityStatModifierData}");
    }
    
    public override string GetDescription()
    {
        return abilityStatModifierData.GetDescription();
    }
}
