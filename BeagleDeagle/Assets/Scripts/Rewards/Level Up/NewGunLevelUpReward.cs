using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewGunLevelUpReward: LevelUpReward
{
    public GunData gunData;
    public NewGunLevelUpReward(GunData data, int level)
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
