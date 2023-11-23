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
    // After the game has paused... invoke this event
    public event Action onGamePause;

    // When the game is resumed after it has been paused... invoke this event
    public event Action onGameResumeAfterPause;

    // When a minute has passed in the game... invoke this event
    public event Action onGameMinutePassed;

    public void InvokeOnGamePauseEvent()
    {
        onGamePause?.Invoke();
    }

    public void InvokeOnGameResumeEvent()
    {
        onGameResumeAfterPause?.Invoke();
    }

    public void InvokeOnGameMinutePassed()
    {
        onGameMinutePassed?.Invoke();
    }
    
}
