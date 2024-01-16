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

    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Waypoint_Indicator _waypointIndicator;
    
    [SerializeField] private AudioClip pickUpSound;

    [SerializeField, Range(0.1f, 1f)] private float volume;

    [SerializeField, Range(1f, 60f)] 
    // How long will this pick up remain on the ground?
    private float pickUpDuration = 30f;

    [TextArea(2,3)]
    public string pickUpDescription;
    public event Action onPickUpDeactivate;
    
    protected virtual void Awake()
    {
        _cooldownSystem = GetComponent<CooldownSystem>();
        _audioClipPlayer = GetComponent<AudioClipPlayer>();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _waypointIndicator = GetComponent<Waypoint_Indicator>();
        
        CooldownDuration = pickUpDuration;
    }

    private void OnEnable()
    {
        _cooldownSystem.OnCooldownEnded += Despawn;
        
    }

    private void OnDisable()
    {
        _cooldownSystem.OnCooldownEnded -= Despawn;
    }

    private void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        
        
    }

    public void ActivatePowerUp()
    {
        _collider2D.enabled = true;
        _spriteRenderer.enabled = true;

        _waypointIndicator.enabled = true;
        
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Activate all effects on food pickup
        if (other.gameObject.CompareTag("Player"))
        {
            _audioClipPlayer.PlayGeneralAudioClip(pickUpSound, volume);
        
            // Display the description of what this pick up should do (if it has one)
            if(pickUpDescription != string.Empty)
                playerEvents.InvokeShowPickUpDescription(pickUpDescription);
            
            HidePowerUp();
            
            OnPickUp(other.gameObject);
            
        }
    }

    protected abstract void OnPickUp(GameObject receiverGameObject);
    
    private void Despawn(int id)
    {
        if (Id != id) return;
        
        Deactivate();
    }

    private void HidePowerUp()
    {
        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
            
        _waypointIndicator.enabled = false;
    }

    protected virtual void Deactivate()
    {
        onPickUpDeactivate?.Invoke();

        Destroy(gameObject);
    }
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}

