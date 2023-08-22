using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// When the player reaches a certain level, they will be given a certain scriptableObject 
/// as an upgrade to a gun, utility ability, or ultimate ability.
/// 
public class Reward<T> where T : ScriptableObject
{
    public T DataToGive;
    
    // What level is the reward given at?
    [Range(1, 20)]
    public int LevelGiven;

    ///-///////////////////////////////////////////////////////////
    /// Return the data that will be given to the player.
    /// 
    public T GetData()
    {
        return DataToGive;
    }
    

}

[System.Serializable]
public class GunReward: Reward<GunData>
{
    public GunReward(GunData data, int level)
    {
        DataToGive = data;
        LevelGiven = level;
    }
}

[System.Serializable]
public class UtilityReward: Reward<UtilityAbilityData>
{

    public UtilityReward(UtilityAbilityData data, int level)
    {
        DataToGive = data;
        LevelGiven = level;
    }
    
}

[System.Serializable]
public class UltimateReward: Reward<UltimateAbilityData>
{

    public UltimateReward(UltimateAbilityData data, int level)
    {
        DataToGive = data;
        LevelGiven = level;
    }
    
}

