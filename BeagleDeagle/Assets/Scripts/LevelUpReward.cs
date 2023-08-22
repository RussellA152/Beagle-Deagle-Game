using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewReward", menuName = "ScriptableObjects/LevelUpReward")]
public class LevelUpReward : ScriptableObject
{
    [NonReorderable]
    public List<GunReward> gunRewards = new List<GunReward>();
    
    [NonReorderable]
    public List<UtilityReward> utilityRewards = new List<UtilityReward>();
    
    [NonReorderable]
    public List<UltimateReward> ultimateRewards = new List<UltimateReward>();

    ///-///////////////////////////////////////////////////////////
    /// Return all gun and ability data rewards that will be given to the player.
    /// All in one list.
    /// 
    // public List<Reward> GetRewardsList()
    // {
    //     List<Reward> rewards = new List<Reward>();
    //     
    //     foreach (GunReward reward in gunRewards)
    //     {
    //         rewards.Add(reward);
    //     }
    //     
    //     foreach (UtilityReward reward in utilityRewards)
    //     {
    //         rewards.Add(reward);
    //     }
    //     
    //     foreach (UltimateReward reward in ultimateRewards)
    //     {
    //         rewards.Add(reward);
    //     }
    //
    //     return rewards;
    // }
    

}
