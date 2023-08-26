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
    
}
