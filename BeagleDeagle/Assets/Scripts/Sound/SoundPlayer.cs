using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private SoundEvents soundEvents;
    
    [Header("Audio Sources Used")] 
    [SerializeField] private AudioSource generalSoundAudioSource;
    [SerializeField] private AudioSource uiAudioSource;

    private void OnEnable()
    {
        soundEvents.onGeneralSoundPlay += PlayGeneralSound;
        soundEvents.onUISoundPlay += PlayUISound;
    }

    private void OnDisable()
    {
        soundEvents.onGeneralSoundPlay -= PlayGeneralSound;
        soundEvents.onUISoundPlay -= PlayUISound;
    }

    ///-///////////////////////////////////////////////////////////
    /// Play sound effects for guns which includes shooting and reloading
    /// 
    private void PlayGeneralSound(AudioClip clipToPlay, float volumeOfClip)
    {
        if (clipToPlay != null)
        {
            generalSoundAudioSource.volume = volumeOfClip;
            generalSoundAudioSource.PlayOneShot(clipToPlay);
        }
        
    }

    ///-///////////////////////////////////////////////////////////
    /// Play sound effects for UI (mainly for button clicks)
    /// 
    private void PlayUISound(AudioClip clipToPlay, float volumeOfClip)
    {
        if (clipToPlay != null)
        {
            uiAudioSource.PlayOneShot(clipToPlay);
        }
        
    }
}
