using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(HealOnCollision))]
public class ConsumablePickUp : MonoBehaviour, IHasCooldown
{
    [SerializeField] private LayerMask whoCanPickup;

    private IStatusEffect[] _statusEffects;
    [SerializeField] private StatusEffectTypes statusEffectData;
    private CooldownSystem _cooldownSystem;

    [SerializeField, Range(1f, 60f)] 
    // How long will this pick up remain on the ground?
    private float pickUpDuration = 30f;
    
    [HideInInspector] public bool retrieved;


    public event Action onPickUpDespawn;
    
    // What does this food do to who ever picked it up?
    public UnityEvent<GameObject> onPickUp;

    private void Awake()
    {
        _statusEffects = GetComponents<IStatusEffect>();
        _cooldownSystem = GetComponent<CooldownSystem>();
        
        Id = 21;
        CooldownDuration = pickUpDuration;
    }

    private void Start()
    {
        foreach (IStatusEffect statusEffect in _statusEffects)
        {
            statusEffect.UpdateWeaponType(statusEffectData);
        }
    }

    private void OnEnable()
    {
        // Reset pick up when enabled
        retrieved = false;
        
        _cooldownSystem.OnCooldownEnded += Despawn;
        
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

    private void OnDisable()
    {
        _cooldownSystem.OnCooldownEnded -= Despawn;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Activate all effects on food pickup
        if ((whoCanPickup.value & (1 << other.gameObject.layer)) > 0)
        {
            onPickUp?.Invoke(other.gameObject);
            
            Despawn(Id);
            
            gameObject.SetActive(false);
        }
    }

    private void Despawn(int id)
    {
        if (Id != id) return;
        onPickUpDespawn?.Invoke();
        
        gameObject.SetActive(false);
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
