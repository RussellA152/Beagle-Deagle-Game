using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelUpReward", menuName = "ScriptableObjects/Rewards/LevelUpReward")]
public class LevelUpRewardList : ScriptableObject
{
    [SerializeField, NonReorderable]
    private List<GunLevelUpReward> gunRewards = new List<GunLevelUpReward>();
    
    [SerializeField, NonReorderable]
    private List<PassiveLevelUpReward> passiveRewads = new List<PassiveLevelUpReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<UtilityLevelUpReward> utilityRewards = new List<UtilityLevelUpReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<UltimateLevelUpReward> ultimateRewards = new List<UltimateLevelUpReward>();

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
        
        foreach (var reward in passiveRewads)
        {
            if (!allRewards.Contains(reward))
            {
                allRewards.Add(reward);
            }
        }
        
    }

}
