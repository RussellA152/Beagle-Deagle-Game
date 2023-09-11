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
        playerEvents.onPlayerLeveledUp += GiveRewardAtLevel;
    }

    private void OnDisable()
    {
        playerEvents.givePlayerGameObject -= FindPlayer;
        playerEvents.onPlayerLeveledUp -= GiveRewardAtLevel;
    }

    // Find the player gameObject which will be given to the rewards so they can give
    // player scripts some scriptableObjects
    private void FindPlayer(GameObject pGameObject)
    {
        _playerGameObject = pGameObject;
    }

    ///-///////////////////////////////////////////////////////////
    /// When player ranks up, give them a reward based on the new level they reached.
    /// 
    private void GiveRewardAtLevel(int newLevel)
    {
        Debug.Log($"Player has reached rank {newLevel}, give them a reward!");
        List<Reward> potentialRewards = new List<Reward>();
        
        // Check all rewards that are given at this level
        foreach (Reward reward in rewardsList.allRewards)
        {
            if (reward.LevelGiven == newLevel)
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
