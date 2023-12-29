using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private GameEvents gameEvents;
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
        if (gameEvents != null)
        {
            gameEvents.onGamePause += PauseAllSoundsWithDuration;
            gameEvents.onGameResumeAfterPause += ResumeAllSoundsWithDuration; 
        }
        
        
        soundEvents.onGeneralSoundPlay += PlayGeneralSound;
        soundEvents.onUISoundPlay += PlayUISound;
        soundEvents.onDurationSoundPlay += PlayDurationSound;
    }

    private void OnDisable()
    {
        if (gameEvents != null)
        {
            gameEvents.onGamePause -= PauseAllSoundsWithDuration;
            gameEvents.onGameResumeAfterPause -= ResumeAllSoundsWithDuration; 
        }
        
        soundEvents.onGeneralSoundPlay -= PlayGeneralSound;
        soundEvents.onUISoundPlay -= PlayUISound;
        soundEvents.onDurationSoundPlay -= PlayDurationSound;
    }
    
    private AudioSource GetAvailableAudioSource()
    {
        // Return an audioSource that isn't currently playing any audio
        foreach (AudioSource audioSource in pooledAudioSources.Keys)
        {
            if (!pooledAudioSources[audioSource])
            {
                pooledAudioSources[audioSource] = true;
                return audioSource;
            }
                
        }

        return null;
    }

    ///-///////////////////////////////////////////////////////////
    /// Play short sound effects for guns which includes shooting and reloading.
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
    /// Play short sound effects for UI (mainly for button clicks).
    /// 
    private void PlayUISound(AudioClip clipToPlay, float volumeOfClip)
    {
        if (clipToPlay != null)
        {
            uiAudioSource.PlayOneShot(clipToPlay);
        }
        
    }

    ///-///////////////////////////////////////////////////////////
    /// Play a sound effect for "durationOfClip" amount of seconds (good for area of effects that have dynamic lifetimes).
    /// 
    private void PlayDurationSound(AudioClip clipToPlay, float volumeOfClip, float duration)
    {
        if (clipToPlay != null)
        {
            StartCoroutine(PlayWhileDuration(clipToPlay, volumeOfClip, duration));
        }
    }

    private IEnumerator PlayWhileDuration(AudioClip clipToPlay, float volumeOfClip, float duration)
    {
        AudioSource audioSourceToUse = GetAvailableAudioSource();

        if (audioSourceToUse == null) yield break;
        
        audioSourceToUse.clip = clipToPlay;
        audioSourceToUse.volume = volumeOfClip;
        audioSourceToUse.loop = true;
        
        audioSourceToUse.Play();
        
        yield return new WaitForSeconds(duration);
        audioSourceToUse.Stop();
        audioSourceToUse.loop = false;
        
        pooledAudioSources[audioSourceToUse] = false;
        
    }

    ///-///////////////////////////////////////////////////////////
    /// When game is paused, pause all duration-based sound effects (ex. area of effect sounds).
    /// 
    private void PauseAllSoundsWithDuration()
    {
        foreach (AudioSource audioSource in pooledAudioSources.Keys)
        {
            if (pooledAudioSources[audioSource])
            {
                audioSource.Pause();
            }
                
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// When the game is unpaused, unpause all duration-based sound effects.
    /// 
    private void ResumeAllSoundsWithDuration()
    {
        foreach (AudioSource audioSource in pooledAudioSources.Keys)
        {
            if (pooledAudioSources[audioSource])
            {
                audioSource.Play();
            }
                
        }
    }
}
