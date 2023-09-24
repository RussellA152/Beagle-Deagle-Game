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
    public Action OnGamePause;

    // When the game is resumed after it has been paused... invoke this event
    public Action OnGameResumeAfterPause;

    public void InvokeOnGamePauseEvent()
    {
        OnGamePause?.Invoke();
    }

    public void InvokeOnGameResumeEvent()
    {
        OnGameResumeAfterPause?.Invoke();
    }
}
