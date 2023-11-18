using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// When performing certain actions such as killing enemies, the player will automatically gain experience
/// which this class will store and notify other scripts about.
/// 
public class PlayerLevelUp : MonoBehaviour
{
    [Header("Data to Use")]
    [SerializeField] private PlayerData playerData;
    
    [SerializeField] private PlayerEvents playerEvents;
    [SerializeField] private EnemyEvents enemyEvents;
    
    // The current rank that the player is at
    private int _currentLevel = 1;

    // How much xp does the player currently have (resets on rank up)
    private int _currentXpUntilNextLevel;

    private bool _allowXpGain = true;

    private void OnEnable()
    {
        enemyEvents.onEnemyDeathXp += GainXp;
    }

    private void OnDisable()
    {
        enemyEvents.onEnemyDeathXp -= GainXp;
    }

    private void Start()
    {
        int xpRequiredForRankUp = playerData.xpNeededPerLevel[_currentLevel - 1];
        
        // Tell all listeners the player's starting level (usually 1), and starting xp (usually 0 xp)
        playerEvents.InvokePlayerLeveledUpEvent(_currentLevel);
        playerEvents.InvokeXpNeededLeftEvent( (float) _currentXpUntilNextLevel/xpRequiredForRankUp);
    }

    ///-///////////////////////////////////////////////////////////
    /// Whenever an enemy dies, gain an "amount" of xp.
    /// Then, invoke an event that the player's current xp count has changed.
    /// 
    public void GainXp(int amount)
    {
        if (!_allowXpGain) return;
        
        _currentXpUntilNextLevel += amount;

        int xpRequiredForRankUp = playerData.xpNeededPerLevel[_currentLevel - 1];
        
        // If the player has more than enough xp needed to level up and still has levels left to reach, then reset their xp and add any difference
        if (_currentLevel < playerData.xpNeededPerLevel.Length && _currentXpUntilNextLevel >= xpRequiredForRankUp)
        {
            _currentXpUntilNextLevel -= xpRequiredForRankUp;
            
            // Tell all listeners that the player has reached a new rank
            playerEvents.InvokePlayerLeveledUpEvent(++_currentLevel);
            
            GiveRewardAtLevel(_currentLevel);
            
        }
        
        // If the player has reached the max rank, then do not allow any further xp gain
        // Also, set currentXpUntilNextLevel equal to the xpRequiredForRankUp (will display a full xp bar at all times)
        if (_currentLevel >= playerData.xpNeededPerLevel.Length)
        {
            _currentXpUntilNextLevel = xpRequiredForRankUp;
            _allowXpGain = false;
        }

        playerEvents.InvokeXpNeededLeftEvent( (float) _currentXpUntilNextLevel/xpRequiredForRankUp);
        
    }
    
    private void GiveRewardAtLevel(int newLevel)
    {
        Debug.Log($"Player has reached rank {newLevel}, give them a reward!");
        
        List<LevelUpReward> optionalRewards = new List<LevelUpReward>();
        
        // Check all rewards that are given at this level
        foreach (LevelUpReward reward in playerData.levelUpRewardsList.allRewards)
        {
            // If player meets the reward's level requirement...
            if (reward.LevelGiven == newLevel)
            {
                // If the reward is optional, add it to "potentialRewards" list
                if (reward.IsChosen)
                {
                    // * The reward that the user chooses will give its data to the player *
                    optionalRewards.Add(reward);
                }
                // Otherwise, have the mandatory reward give its data to the player
                else
                {
                    reward.GiveDataToPlayer(gameObject);
                    
                    // UI will display the description of this mandatory reward
                    playerEvents.InvokePlayerReceivedMandatoryRewardEvent(reward);
                }
                
            }
        }
        // If there are optional rewards to give, then tell the UI to display the choices
        if(optionalRewards.Count > 0)
            playerEvents.InvokePlayerReceivedPotentialRewardsEvent(optionalRewards);
    }

}
