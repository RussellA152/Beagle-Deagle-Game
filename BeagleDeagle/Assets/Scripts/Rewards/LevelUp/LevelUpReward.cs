using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


///-///////////////////////////////////////////////////////////
/// When the player reaches a certain level, they will be given a certain scriptableObject 
/// as an upgrade to a gun, utility ability, or ultimate ability.
/// 
public abstract class LevelUpReward
{
    [Range(1, 20),Tooltip("What level is the reward given at?")]
    public int LevelGiven;
    
    [Tooltip("Will this reward be one of many other choices when the player reaches the level requirement?")]
    public bool IsChosen;

    public Image Icon;

    [Space(10), TextArea(2,3)]
    public string Description;

    public abstract string GetRewardName();
    public abstract void GiveDataToPlayer(GameObject recipientGameObject);

}

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
}

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
        return "Utility Ability Upgrade";
    }
    
    public override void GiveDataToPlayer(GameObject recipientGameObject)
    {
        recipientGameObject.GetComponent<IUtilityUpdatable>().UpdateScriptableObject(utilityAbilityData);
        Debug.Log($"{recipientGameObject.name} was given {utilityAbilityData}");
    }
}

[System.Serializable]
public class UltimateLevelUpReward: LevelUpReward
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
    
}

