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

    private void PlayGunSound(AudioClip clipToPlay)
    {
        gunSoundAudioSource.PlayOneShot(clipToPlay);
    }

    private void PlayUISound(AudioClip clipToPlay)
    {
        uiAudioSource.PlayOneShot(clipToPlay);
    }
}
