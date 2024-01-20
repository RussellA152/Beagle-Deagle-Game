using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewLevelUpReward", menuName = "ScriptableObjects/Rewards/LevelUpReward")]
public class LevelUpRewardList : ScriptableObject
{
    [SerializeField, NonReorderable, Space(20)]
    private List<NewGunLevelUpReward> gunRewards = new List<NewGunLevelUpReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<WeaponStatLevelUpReward> weaponStatRewards = new List<WeaponStatLevelUpReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<PassiveLevelUpReward> passiveRewards = new List<PassiveLevelUpReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<UtilityLevelUpReward> utilityRewards = new List<UtilityLevelUpReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<UltimateLevelUpReward> ultimateRewards = new List<UltimateLevelUpReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<AbilityStatLevelUpReward> abilityStatRewards = new List<AbilityStatLevelUpReward>();
    
    [SerializeField, NonReorderable]
    private List<MiscellaneousStatLevelUpReward> miscellaneousStatRewards = new List<MiscellaneousStatLevelUpReward>();

    public readonly List<LevelUpReward> allRewards = new List<LevelUpReward>();

    // Add all gun, utility ability, and ultimate ability scriptable objects to 1 list
    private void OnEnable()
    {
        allRewards.Clear();

        foreach (var reward in gunRewards)
        {
            if (!allRewards.Contains(reward))
            {
                allRewards.Add(reward);
            }
        }
        
        foreach (var reward in weaponStatRewards)
        {
            if (!allRewards.Contains(reward))
            {
                allRewards.Add(reward);
            }
        }
        
        foreach (var reward in passiveRewards)
        {
            if (!allRewards.Contains(reward))
            {
                allRewards.Add(reward);
            }
        }
    
        foreach (var reward in utilityRewards)
        {
            if (!allRewards.Contains(reward))
            {
                allRewards.Add(reward);
            }
        }
    
        foreach (var reward in ultimateRewards)
        {
            if (!allRewards.Contains(reward))
            {
                allRewards.Add(reward);
            }
        }
        
        foreach (var reward in abilityStatRewards)
        {
            if (!allRewards.Contains(reward))
            {
                allRewards.Add(reward);
            }
        }

        foreach (var reward in miscellaneousStatRewards)
        {
            if (!allRewards.Contains(reward))
            {
                allRewards.Add(reward);
            }
        }

    }

}
