using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewReward", menuName = "ScriptableObjects/LevelUpReward")]
public class LevelUpReward : ScriptableObject
{
    [NonReorderable]
    public List<Reward> levelRewards = new List<Reward>();
    
}

[System.Serializable]
public class Reward
{
    public ScriptableObject dataToGive;
    public int levelGiven;

    public Reward(ScriptableObject data, int level)
    {
        dataToGive = data;
        levelGiven = level;
    }
}
