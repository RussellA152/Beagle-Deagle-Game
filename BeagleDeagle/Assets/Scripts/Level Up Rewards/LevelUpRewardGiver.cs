using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelUpRewardGiver : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private RewardList rewardsList;

    private GameObject _playerGameObject;

    [SerializeField] private RewardSelectionUI rewardSelectionUI;
    
    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += FindPlayer;
    }

    private void Start()
    {
        //_rewardsToGive = levelUpRewards.GetRewardsList();
        Invoke(nameof(GiveRewardAtLevel), 3f);


    }

    // Find the player gameObject which will be given to the rewards so they can give
    // player scripts some scriptableObjects
    private void FindPlayer(GameObject pGameObject)
    {
        _playerGameObject = pGameObject;
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
        foreach (Reward reward in rewardsList.allRewards)
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

    ///-///////////////////////////////////////////////////////////
    /// If the reward was optional, then tell RewardChoiceUI script to display all optional
    /// rewards on a UI panel for the player to click and choose.
    private void ChooseReward(List<Reward> list)
    {
        foreach (Reward potentialReward in list)
        {
            Debug.Log($"{potentialReward.Description} is a potential reward for the player.");
            
            rewardSelectionUI.AddChoiceButton(potentialReward);
        }
    }
    
}
