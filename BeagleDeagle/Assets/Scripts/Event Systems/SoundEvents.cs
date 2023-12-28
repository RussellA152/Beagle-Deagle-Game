using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// A scriptableObject that classes that want to play sounds will invoke the events for. A sound player will exist in
/// the scene that will play the sound effects.
[CreateAssetMenu(menuName = "GameEvent/SoundEvents")]
public class SoundEvents : ScriptableObject
{
    public event Action<AudioClip, float> onGeneralSoundPlay;
    public event Action<AudioClip, float> onUISoundPlay;

    public event Action<AudioClip, float, float> onDurationSoundPlay; 

    public void InvokeGeneralSoundPlay(AudioClip clip, float volumeOfClip)
    {
        onGeneralSoundPlay?.Invoke(clip, volumeOfClip);
    }

    public void InvokeUISoundPlay(AudioClip clip, float volumeOfClip)
    {
        onUISoundPlay?.Invoke(clip, volumeOfClip);
    }

    public void InvokePlayWhileDuration(AudioClip clip, float volumeOfClip, float duration)
    {
        onDurationSoundPlay?.Invoke(clip, volumeOfClip, duration);
    }
}
