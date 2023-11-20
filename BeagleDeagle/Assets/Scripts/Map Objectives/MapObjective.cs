using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

///-///////////////////////////////////////////////////////////
/// Map Objectives are optional, timed activities in the game level besides the main activity of surviving the timer.
/// Completing an objective will reward the player with xp and items.
/// 
public abstract class MapObjective : MonoBehaviour, IHasCooldown
{
    [SerializeField] private GameEvents gameEvents;

    // Is this objective currently being played?
    public bool IsActive { get; private set; }

    // How long will the player play this objective? (ex. Survive for "duration" seconds)
    [SerializeField, Range(1f, 180f)] 
    private float duration;

    [SerializeField] private CurrencyReward completionReward;

    protected CooldownSystem CooldownSystem;

    // Collider that player needs to touch to begin objective
    private CapsuleCollider2D _startObjectiveCollider;

    private void Awake()
    {
        Id = 50;
        CooldownSystem = GetComponent<CooldownSystem>();
        CooldownDuration = duration;

        _startObjectiveCollider = GetComponent<CapsuleCollider2D>();
        
    }

    private void OnEnable()
    {
        CooldownSystem.OnCooldownEnded += ObjectiveExpire;
        
        OnObjectiveEnable();
    }

    private void OnDisable()
    {
        // Re-activate start collider
        _startObjectiveCollider.enabled = true;
        
        CooldownSystem.OnCooldownEnded -= ObjectiveExpire;

        OnObjectiveDisable();
    }

    protected virtual void OnObjectiveEnable()
    {
        //IsActive = true;
    }

    protected virtual void OnObjectiveDisable()
    {
        IsActive = false;
    }

    ///-///////////////////////////////////////////////////////////
    /// When completing the objective, give the player a reward.
    /// Then do something abstract afterwards (ex. destroy objects that are no longer needed)
    /// 
    protected virtual void OnObjectiveCompletion()
    {
        // Give reward
        gameEvents.InvokeMapObjectiveCompletedEvent(completionReward);
        
        MapObjectiveManager.instance.StartObjectiveAfterCompletion(this);
        
    }

    private void ObjectiveExpire(int cooldownId)
    {
        if (Id == cooldownId)
        {
            IsActive = false;
            MapObjectiveManager.instance.StartObjectiveAfterExpire(this);
            Debug.Log("ACTIVITY EXPIRED!");
        }
           
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsActive = true;
            
            // Start objective when player enters 
            Debug.Log("Start objective timer!");
            CooldownSystem.PutOnCooldown(this);

            // Disable start collider when player enters
            _startObjectiveCollider.enabled = false;
        }
        
    }

    public int Id { get; set; }
    
    public float CooldownDuration { get; set; }
}
