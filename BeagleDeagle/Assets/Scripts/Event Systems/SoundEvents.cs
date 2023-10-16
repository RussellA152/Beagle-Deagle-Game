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
    public event Action<AudioClip, float> onGunSoundPlay;
    
    public event Action<AudioClip, float> onUISoundPlay;

    public void InvokeGunSoundPlay(AudioClip clip, float volumeOfClip)
    {
        onGunSoundPlay?.Invoke(clip, volumeOfClip);
    }
    public void InvokeUISoundPlay(AudioClip clip, float volumeOfClip)
    {
        onUISoundPlay?.Invoke(clip, volumeOfClip);
    }
}
