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

    [SerializeField] private GameObject audioSourcePooledItem;
    [SerializeField, Range(5, 30)] 
    private int amountToPool = 20;

    // Key: AudioSource, Value: inUse (playing audio)
    private Dictionary<AudioSource, bool> pooledAudioSources = new Dictionary<AudioSource, bool>();

    private void Awake()
    {
        // Make multiple audio sources to pool
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject pooledAudioSource =  Instantiate(audioSourcePooledItem, transform);
            
            pooledAudioSources.Add(pooledAudioSource.GetComponent<AudioSource>(), false);
        }
    }

    private void OnEnable()
    {
        soundEvents.onGeneralSoundPlay += PlayGeneralSound;
        soundEvents.onUISoundPlay += PlayUISound;
        soundEvents.onDurationSoundPlay += PlayDurationSound;
    }

    private void OnDisable()
    {
        soundEvents.onGeneralSoundPlay -= PlayGeneralSound;
        soundEvents.onUISoundPlay -= PlayUISound;
        soundEvents.onDurationSoundPlay -= PlayDurationSound;
    }
    
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource audioSource in pooledAudioSources.Keys)
        {
            // Return an audioSource that isn't currently playing any audio
            if (!pooledAudioSources[audioSource])
            {
                pooledAudioSources[audioSource] = true;
                return audioSource;
            }
                
        }

        return null;
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

    private void PlayDurationSound(AudioClip clipToPlay, float volumeOfClip, float durationOfClip)
    {
        if (clipToPlay != null)
        {
            StartCoroutine(PlayWhileDuration(clipToPlay, volumeOfClip, durationOfClip));
        }
    }

    private IEnumerator PlayWhileDuration(AudioClip clipToPlay, float volumeOfClip, float durationOfClip)
    {
        AudioSource audioSourceToUse = GetAvailableAudioSource();

        if (audioSourceToUse == null) yield break;

        audioSourceToUse.clip = clipToPlay;
        audioSourceToUse.volume = volumeOfClip;
        
        audioSourceToUse.Play();
        
        yield return new WaitForSeconds(durationOfClip);
        audioSourceToUse.Stop();
        pooledAudioSources[audioSourceToUse] = false;
        
    }
}
