using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UtilityLevelUpReward: LevelUpReward
{
    public UtilityAbilityData utilityAbilityData;
    
    public UtilityLevelUpReward(UtilityAbilityData data, int level)
    {
        utilityAbilityData = data;
        LevelGiven = level;
    }
    public override string GetRewardName()
    {
        return utilityAbilityData.abilityName;
    }
    
    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        recipientGameObject.GetComponent<IUtilityUpdatable>().UpdateScriptableObject(utilityAbilityData);
        Debug.Log($"{recipientGameObject.name} was given {utilityAbilityData}");
    }
    
    public override string GetDescription()
    {
        return utilityAbilityData.GetDescription();
    }
}
