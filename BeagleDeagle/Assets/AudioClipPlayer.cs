using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
    [SerializeField] private SoundEvents soundEvents;

    public void PlayGeneralAudioClip(AudioClip audioClip, float volume)
    {
        if (audioClip != null)
        {
            soundEvents.InvokeGeneralSoundPlay(audioClip, volume);
        }
    }

    public void PlayForDurationAudioClip(AudioClip audioClip, float volume, float duration)
    {
        if (audioClip != null)
        {
            soundEvents.InvokePlayWhileDuration(audioClip, volume, duration);
        }
    }
    
    public void PlayRandomGeneralAudioClip(AudioClip[] audioClips, float volume)
    {
        if (audioClips.Length > 0)
        {
            int randomNumber = Random.Range(0, audioClips.Length);
        
            soundEvents.InvokeGeneralSoundPlay(audioClips[randomNumber], volume);
            
        }
    }
}
