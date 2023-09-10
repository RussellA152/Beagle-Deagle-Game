using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewReward", menuName = "ScriptableObjects/LevelUpReward")]
public class RewardList : ScriptableObject
{
    [SerializeField, NonReorderable]
    private List<GunReward> gunRewards = new List<GunReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<UtilityReward> utilityRewards = new List<UtilityReward>();
    
    [SerializeField, NonReorderable, Space(20)]
    private List<UltimateReward> ultimateRewards = new List<UltimateReward>();

    public readonly List<Reward> allRewards = new List<Reward>();

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
    }

}
