using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


///-///////////////////////////////////////////////////////////
/// When the player reaches a certain level, they will be given a certain scriptableObject 
/// as an upgrade to a gun, utility ability, or ultimate ability.
/// 
public abstract class LevelUpReward: IHasDescription
{
    [Range(1, 20),Tooltip("What level is the reward given at?")]
    public int LevelGiven;
    
    [Tooltip("Will this reward be one of many other choices when the player reaches the level requirement?")]
    public bool IsChosen;

    public Image Icon;

    public abstract string GetRewardName();
    public abstract void GiveDataToPlayer(GameObject recipientGameObject);

    public abstract string GetDescription();

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

    public override string GetDescription()
    {
        return gunData.GetDescription();
    }
}

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
        return "Ultimate Ability Upgrade";
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
    
    public override string GetDescription()
    {
        return utilityAbilityData.GetDescription();
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
    
    public override string GetDescription()
    {
        return ultimateAbilityData.GetDescription();
    }
    
}

