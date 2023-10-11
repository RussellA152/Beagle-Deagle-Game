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
    public event Action<AudioClip> onGunSoundPlay;
    
    public event Action<AudioClip> onUISoundPlay;

    public void InvokeGunSoundPlay(AudioClip clip)
    {
        onGunSoundPlay?.Invoke(clip);
    }
    public void InvokeUISoundPlay(AudioClip clip)
    {
        onUISoundPlay?.Invoke(clip);
    }
}
