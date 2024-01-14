using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ConsumablePickUp : MonoBehaviour, IHasCooldown
{
    [SerializeField] private LayerMask whoCanPickup;

    private CooldownSystem _cooldownSystem;
    
    // Can play sounds on pick up
    private AudioClipPlayer _audioClipPlayer;
    
    [SerializeField] private AudioClip pickUpSound;

    [SerializeField, Range(0.1f, 1f)] private float volume;
    

    [SerializeField, Range(1f, 60f)] 
    // How long will this pick up remain on the ground?
    private float pickUpDuration = 30f;
    
    public event Action onPickUpDespawn;
    
    // What does this food do to who ever picked it up?
    public UnityEvent<GameObject> onPickUp;

    private void Awake()
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
        if ((whoCanPickup.value & (1 << other.gameObject.layer)) > 0)
        {
            onPickUp?.Invoke(other.gameObject);
            
            _audioClipPlayer.PlayGeneralAudioClip(pickUpSound, volume);
            
            Despawn(Id);
        }
    }

    private void Despawn(int id)
    {
        if (Id != id) return;
        
        Debug.Log("Despawn pickup! " + gameObject.name);
        
        onPickUpDespawn?.Invoke();
        
        gameObject.SetActive(false);
    }
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}

