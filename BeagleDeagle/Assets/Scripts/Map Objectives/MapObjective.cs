using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// Map Objectives are optional, timed activities in the game level besides the main activity of surviving the timer.
/// Completing an objective will reward the player with xp and items.
/// 
public abstract class MapObjective : MonoBehaviour
{
    [SerializeField] private GameEvents gameEvents;
    // How long will it take for this objective to expire? (disappear after being ignored)
    [SerializeField] private float timeUntilExpire;

    // How long will the player play this objective? (ex. Survive for "duration" seconds)
    [SerializeField] private float duration;

    [SerializeField] private CurrencyReward currencyRewardOnCompletion;

    // Rewards given for completing this objective
    //[SerializeField] private RewardList completionRewards;

    private void OnEnable()
    {
        SetUpObjective();
    }

    private void OnDisable()
    {
        OnObjectiveDisable();
    }

    ///-///////////////////////////////////////////////////////////
    /// Start this objective and set up any tasks needed for the player 
    /// to interact with this objective
    /// 
    protected abstract void SetUpObjective();
    
    protected abstract void OnObjectiveDisable();

    ///-///////////////////////////////////////////////////////////
    /// When completing the objective, give the player a reward.
    /// Then do something abstract afterwards (ex. destroy objects thats are no longer needed)
    /// 
    protected virtual void OnObjectiveCompletion()
    {
        // Give reward
        gameEvents.InvokeMapObjectiveCompletedEvent(currencyRewardOnCompletion);
        
    }
    

}
