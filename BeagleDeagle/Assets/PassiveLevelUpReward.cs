using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveLevelUpReward: LevelUpReward
{
    public PassiveAbilityData passiveAbilityData;
    public PassiveLevelUpReward(PassiveAbilityData data, int level)
    {
        passiveAbilityData = data;
        LevelGiven = level;
    }

    public override string GetRewardName()
    {
        return "New Passive Ability";
    }
    
    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        recipientGameObject.GetComponentInChildren<PassiveInventory>().GetNewPassive(passiveAbilityData);
        Debug.Log($"{recipientGameObject.name} was given {passiveAbilityData}");
    }
    
    public override string GetDescription()
    {
        return passiveAbilityData.GetDescription();
    }
    
}
