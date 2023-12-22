using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UltimateLevelUpReward : LevelUpReward
{
    public UltimateAbilityData ultimateAbilityData;

    public UltimateLevelUpReward(UltimateAbilityData data, int level)
    {
        ultimateAbilityData = data;
        LevelGiven = level;
    }

    public override string GetRewardName()
    {
        return "Ultimate Ability Upgrade";
    }

    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        recipientGameObject.GetComponent<IUltimateUpdatable>().UpdateScriptableObject(ultimateAbilityData);
        Debug.Log($"{recipientGameObject.name} was given {ultimateAbilityData}");
    }

    public override string GetDescription()
    {
        return ultimateAbilityData.GetDescription();
    }
}
