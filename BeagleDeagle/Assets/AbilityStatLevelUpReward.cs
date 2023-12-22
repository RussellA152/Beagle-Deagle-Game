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
        IUtilityUpdatable utilityScript = recipientGameObject.GetComponent<IUtilityUpdatable>();
        IUltimateUpdatable ultimateScript = recipientGameObject.GetComponent<IUltimateUpdatable>();
        
        if(abilityStatModifierData.UtilityUsesModifier != null)
            utilityScript.AddUtilityUsesModifier(abilityStatModifierData.UtilityUsesModifier);
        
        if(abilityStatModifierData.UtilityCooldownModifier != null)
            utilityScript.AddUtilityCooldownModifier(abilityStatModifierData.UtilityCooldownModifier);
        
        if(abilityStatModifierData.UltimateCooldownModifier != null)
            ultimateScript.AddUltimateCooldownModifier(abilityStatModifierData.UltimateCooldownModifier);
        
        Debug.Log($"{recipientGameObject.name} was given {abilityStatModifierData}");
    }
    
    public override string GetDescription()
    {
        return abilityStatModifierData.GetDescription();
    }
}
