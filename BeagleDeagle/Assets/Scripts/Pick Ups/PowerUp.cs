using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class PowerUp : MonoBehaviour, IHasCooldown
{
    [SerializeField] protected PlayerEvents playerEvents;

    [SerializeField] private Waypoint_Indicator waypointIndicator;
    private CooldownSystem _cooldownSystem;

    // Can play sounds on pick up
    private AudioClipPlayer _audioClipPlayer;

    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private AudioClip pickUpSound;

    [SerializeField, Range(0.1f, 1f)] private float volume;

    [SerializeField, Range(1f, 60f)] 
    // How long will this pick up remain on the ground?
    private float pickUpDuration = 30f;

    [TextArea(2,3)]
    public string pickUpDescription;

    [SerializeField]
    private bool destroyOnDeactivation;
    [SerializeField] private bool activateOnStart;
    public event Action onPickUpDeactivate;
    
    protected virtual void Awake()
    {
        _cooldownSystem = GetComponent<CooldownSystem>();
        _audioClipPlayer = GetComponent<AudioClipPlayer>();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        CooldownDuration = pickUpDuration;
    }
    

    private void Start()
    {
        Id = _cooldownSystem.GetAssignableId();

        if (activateOnStart)
        {
            ActivatePowerUp();
        }
    }

    public void ActivatePowerUp()
    {
        _cooldownSystem.OnCooldownEnded += Despawn;
        
        ShowPowerUp();

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
            
            OnPickUp(other.gameObject);

            HidePowerUp();
            
            _cooldownSystem.RemoveCooldown(Id);
            
        }
    }

    protected abstract void OnPickUp(GameObject receiverGameObject);
    
    private void Despawn(int id)
    {
        if (Id != id) return;
        
        Deactivate();
    }

    private void ShowPowerUp()
    {
        waypointIndicator.enabled = true;
        _collider2D.enabled = true;
        _spriteRenderer.enabled = true;
    }

    private void HidePowerUp()
    {
        waypointIndicator.enabled = false;
        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
    }

    protected virtual void Deactivate()
    {
        HidePowerUp();
        
        _cooldownSystem.OnCooldownEnded -= Despawn;
        
        onPickUpDeactivate?.Invoke();

        if(destroyOnDeactivation)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}

