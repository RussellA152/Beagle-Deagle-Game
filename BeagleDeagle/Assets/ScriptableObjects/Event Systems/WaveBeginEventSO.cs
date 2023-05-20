using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "GameEvent/OnWaveBegin")]
public class WaveBeginEventSO : ScriptableObject
{
    [NonSerialized]
    public UnityEvent<string> changeHUDTextEvent;

    private void OnEnable()
    {
        if (changeHUDTextEvent == null)
            changeHUDTextEvent = new UnityEvent<string>();
    }

    public void InvokeEvent(string text)
    {
        changeHUDTextEvent.Invoke(text);
    }
}
