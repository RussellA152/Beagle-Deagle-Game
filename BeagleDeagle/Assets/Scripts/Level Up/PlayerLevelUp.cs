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

    private void OnEnable()
    {
        enemyEvents.onEnemyDeathXp += GainXpFromEnemyKill;
    }

    private void OnDisable()
    {
        enemyEvents.onEnemyDeathXp -= GainXpFromEnemyKill;
    }

    ///-///////////////////////////////////////////////////////////
    /// Whenever an enemy dies, gain an "amount" of xp.
    /// Then, invoke an event that the player's current xp count has changed.
    /// 
    private void GainXpFromEnemyKill(int amount)
    {
        _currentXpUntilNextLevel += amount;

        int xpRequiredForRankUp = playerData.xpNeededPerLevel[_currentLevel - 1];
        
        // If the player has more than enough xp needed to level up, then reset their xp and add any difference
        if (_currentXpUntilNextLevel >= xpRequiredForRankUp)
        {
            _currentXpUntilNextLevel -= xpRequiredForRankUp;
            
            // Tell all listeners that the player has reached a new rank
            playerEvents.InvokePlayerLeveledUpEvent(++_currentLevel);
        }
        
        playerEvents.InvokeXpNeededLeftEvent( (float) _currentXpUntilNextLevel/xpRequiredForRankUp);
    }

}
