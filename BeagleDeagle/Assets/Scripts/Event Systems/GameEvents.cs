using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// A collection of significant event systems for managing important events that occur during gameplay,
/// such as game over, pausing and unpausing the game, and other key events.
///
[CreateAssetMenu(menuName = "GameEvent/GameEvents")]
public class GameEvents : ScriptableObject
{
    // When a minute has passed in the game... invoke this event
    public event Action onGameMinutePassed;
    
    
    // When the level's play timer has reached the max (ex. 20 minutes)... invoke this event
    public event Action onGameTimeConcluded;
    
    // After the game has paused... invoke this event
    public event Action onGamePause;

    // When the game is resumed after it has been paused... invoke this event
    public event Action onGameResumeAfterPause;
    
    
    // When a map objective was started by the player... invoke this event
    public event Action<MapObjective> onMapObjectiveBegin;
    
    // When a map objective has ended (after being started by player)... invoke this event 
    public event Action<MapObjective> onMapObjectiveEnded;

    public void InvokeGameMinutePassedEvent()
    {
        onGameMinutePassed?.Invoke();
    }

    public void InvokeGameTimeConcludedEvent()
    {
        onGameTimeConcluded?.Invoke();
    }
    
    public void InvokeGamePauseEvent()
    {
        onGamePause?.Invoke();
    }

    public void InvokeGameResumeEvent()
    {
        onGameResumeAfterPause?.Invoke();
    }
    

    public void InvokeMapObjectiveBeginEvent(MapObjective mapObjectiveStarted)
    {
        onMapObjectiveBegin?.Invoke(mapObjectiveStarted);
    }
    
    public void InvokeMapObjectiveEndedEvent(MapObjective mapObjectiveEnded)
    {
        onMapObjectiveEnded?.Invoke(mapObjectiveEnded);
    }
    
}
