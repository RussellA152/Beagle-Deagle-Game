using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class PowerUp : MonoBehaviour, IHasCooldown
{
    [SerializeField] protected PlayerEvents playerEvents;

    private CooldownSystem _cooldownSystem;

    // Can play sounds on pick up
    private AudioClipPlayer _audioClipPlayer;
    
    [SerializeField] private AudioClip pickUpSound;

    [SerializeField, Range(0.1f, 1f)] private float volume;
    
    [SerializeField, Range(1f, 60f)] 
    // How long will this pick up remain on the ground?
    private float pickUpDuration = 30f;

    [TextArea(2,3)]
    public string pickUpDescription;
    public event Action onPickUpDespawn;
    
    protected virtual void Awake()
    {
        _cooldownSystem = GetComponent<CooldownSystem>();
        _audioClipPlayer = GetComponent<AudioClipPlayer>();
        
        CooldownDuration = pickUpDuration;
    }

    private void OnEnable()
    {
        _cooldownSystem.OnCooldownEnded += Despawn;
        
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = pickUpDuration;

        // Start timer until disappear
        if (!_cooldownSystem.IsOnCooldown(Id))
        {
            _cooldownSystem.PutOnCooldown(this);
        }
        else
        {
            _cooldownSystem.RefreshCooldown(Id);
        }
    }

    private void OnDestroy()
    {
        _cooldownSystem.OnCooldownEnded -= Despawn;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Activate all effects on food pickup
        if (other.gameObject.CompareTag("Player"))
        {
            _audioClipPlayer.PlayGeneralAudioClip(pickUpSound, volume);
        
            // Display the description of what this pick up should do (if it has one)
            if(pickUpDescription != string.Empty)
                playerEvents.InvokeShowPickUpDescription(pickUpDescription);

            OnPickUp(other.gameObject);
            
            Despawn(Id);
        }
    }

    protected abstract void OnPickUp(GameObject receiverGameObject);
    
    private void Despawn(int id)
    {
        if (Id != id) return;

        onPickUpDespawn?.Invoke();
        
        gameObject.SetActive(false);
    }
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}

