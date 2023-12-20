using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunLevelUpReward: LevelUpReward
{
    public GunData gunData;
    public GunLevelUpReward(GunData data, int level)
    {
        gunData = data;
        LevelGiven = level;
        
    }

    public override string GetRewardName()
    {
        return "Weapon Upgrade";
    }

    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        recipientGameObject.GetComponentInChildren<IGunDataUpdatable>().UpdateScriptableObject(gunData);
        Debug.Log($"{recipientGameObject.name} was given {gunData}");
    }

    public override string GetDescription()
    {
        return gunData.GetDescription();
    }
}
