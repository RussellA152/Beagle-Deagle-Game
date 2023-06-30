using System;
using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent/OnWaveBegin")]
public class WaveBeginEvents : ScriptableObject
{
    public event Action<string> changeHUDTextEvent;

    public void InvokeEvent(string text)
    {
        if(changeHUDTextEvent != null)
        {
            changeHUDTextEvent(text);
        }
    }
}
