using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private SoundEvents soundEvents;
    
    [Header("Audio Sources Used")] 
    [SerializeField] private AudioSource gunSoundAudioSource;
    [SerializeField] private AudioSource uiAudioSource;

    private void OnEnable()
    {
        soundEvents.onGunSoundPlay += PlayGunSound;
        soundEvents.onUISoundPlay += PlayUISound;
    }

    private void OnDisable()
    {
        soundEvents.onGunSoundPlay -= PlayGunSound;
        soundEvents.onUISoundPlay -= PlayUISound;
    }

    ///-///////////////////////////////////////////////////////////
    /// Play sound effects for guns which includes shooting and reloading
    /// 
    private void PlayGunSound(AudioClip clipToPlay, float volumeOfClip)
    {
        gunSoundAudioSource.volume = volumeOfClip;
        gunSoundAudioSource.PlayOneShot(clipToPlay);
    }

    ///-///////////////////////////////////////////////////////////
    /// Play sound effects for UI (mainly for button clicks)
    /// 
    private void PlayUISound(AudioClip clipToPlay, float volumeOfClip)
    {
        uiAudioSource.PlayOneShot(clipToPlay);
    }
}
