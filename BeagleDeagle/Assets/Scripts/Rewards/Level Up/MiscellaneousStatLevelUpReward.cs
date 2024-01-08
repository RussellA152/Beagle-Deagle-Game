using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class MiscellaneousStatLevelUpReward: LevelUpReward
{
     public MiscellaneousModifierData miscellaneousModifierData;
    
    public MiscellaneousStatLevelUpReward(MiscellaneousModifierData data, int level)
    {
        miscellaneousModifierData = data;
        LevelGiven = level;
    }

    public override string GetRewardName()
    {
        return "Miscellaneous Stat Modifier";
    }
    
    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        MiscellaneousModifierList miscellaneousModifierList = recipientGameObject.GetComponent<MiscellaneousModifierList>();
        
        if(miscellaneousModifierData.explosiveRadiusModifier.IsModifierNameValid())
            miscellaneousModifierList.AddExplosiveRadiusModifier(miscellaneousModifierData.explosiveRadiusModifier);
        
        Debug.Log($"{recipientGameObject.name} was given {miscellaneousModifierData}");
    }
    
    public override string GetDescription()
    {
        return miscellaneousModifierData.GetDescription();
    }
    
}
