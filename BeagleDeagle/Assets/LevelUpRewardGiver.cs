using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpRewardGiver : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private LevelUpReward levelUpRewards;

    private GameObject _playerGameObject;

    // Scripts that the Giver will give scriptableObjects to
    private IGunDataUpdatable _gunScript;
    private IUtilityUpdatable _utilityScript;
    private IUltimateUpdatable _ultimateScript;
    

    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += FindPlayer;
    }

    private void Start()
    {
        //_rewardsToGive = levelUpRewards.GetRewardsList();
        Invoke(nameof(GiveRewardAtLevel), 3f);
        
        
    }

    private void FindPlayer(GameObject pGameObject)
    {
        _playerGameObject = pGameObject;
        
        
        Debug.Log(_playerGameObject.GetComponentInChildren<IDataUpdatable<GunData>>());

        _gunScript = _playerGameObject.GetComponentInChildren<IGunDataUpdatable>();

        _utilityScript = _playerGameObject.GetComponent<IUtilityUpdatable>();

        _ultimateScript = _playerGameObject.GetComponent<IUltimateUpdatable>();
    }

    ///-///////////////////////////////////////////////////////////
    /// When the player levels up, check if their rewards list has a reward at that level.
    /// If so, give them that reward.
    /// 
    // private void GiveRewardAtLevel(int newLevel)
    // {
    //     foreach (Reward levelReward in levelUpRewards.levelRewards)
    //     {
    //         if (newLevel == levelReward.levelGiven)
    //         {
    //             GiveRewardToPlayer(levelReward);
    //         }
    //         
    //     }
    // }
    
    // TODO: Use above method when player ranks up with event system!
    private void GiveRewardAtLevel()
    {
        List<Reward> potentialRewards = new List<Reward>();
        foreach (Reward reward in levelUpRewards.allRewards)
        {
            if (reward.LevelGiven == 5)
            {
                if (reward.IsChosen)
                {
                    potentialRewards.Add(reward);
                }
                else
                {
                    reward.GiveDataToPlayer(_playerGameObject);
                }
                
            }
        }
        ChooseReward(potentialRewards);
    }

    private void ChooseReward(List<Reward> rewardsList)
    {
        foreach (Reward potentialReward in rewardsList)
        {
            Debug.Log($"{potentialReward.Description} is a potential reward for the player.");
        }
    }
    
}
