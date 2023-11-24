using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

///-///////////////////////////////////////////////////////////
/// Map Objectives are optional, timed activities in the game level besides the main activity of surviving the timer.
/// Completing an objective will reward the player with xp and items.
/// 
public abstract class MapObjective : MonoBehaviour, IHasCooldown
{
    [SerializeField] private CurrencyEvents currencyEvents;

    [SerializeField] private List<Transform> potentialSpawnLocations = new List<Transform>();

    private Transform _currentSpawnLocation;
    private Transform _previousSpawnLocation;
    
    [SerializeField] private CurrencyReward completionReward;
    
    // How long will the player play this objective? (ex. Survive for "duration" seconds)
    [SerializeField, Range(1f, 180f)] 
    private float timeAllotted;

    protected CooldownSystem CooldownSystem;
    private MapObjectiveExpire _mapObjectiveExpire;
    
    // Is this objective currently being played?
    public bool IsActive { get; private set; }

    private void Awake()
    {
        OnObjectiveAwake();
    }

    private void Start()
    {
        CooldownSystem.OnCooldownEnded += ObjectiveOutOfTime;
    }

    private void OnEnable()
    {
        OnObjectiveEnable();
    }

    private void OnDisable()
    {
        OnObjectiveDisable();
    }

    private void OnDestroy()
    {
        CooldownSystem.OnCooldownEnded -= ObjectiveOutOfTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnObjectiveEnter();
        }
        
    }

    protected virtual void OnObjectiveAwake()
    {
        Id = 50;
        CooldownSystem = GetComponent<CooldownSystem>();
        CooldownDuration = timeAllotted;

        _mapObjectiveExpire = GetComponent<MapObjectiveExpire>();
    }

    protected virtual void OnObjectiveEnable()
    {
        PlaceObjectiveAtRandomLocation();
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
        currencyEvents.InvokeGiveXp(completionReward.xpAmount);
        
        MapObjectiveManager.Instance.StartNewObjectiveAfterEnded(this);

    }

    // All objectives will remove their cooldown on completion except for the DefendObjective.
    // This is because the DefendObjective must run out of time to be considered completed, which results
    // in the cooldown being removed anyways.
    protected void RemoveCooldown()
    {
        if(CooldownSystem.IsOnCooldown(Id))
            CooldownSystem.RemoveCooldown(Id);
    }

    
    protected virtual void OnObjectiveEnter()
    {
        // If the objective is already active, don't try to activate again
        if (IsActive) return;
        
        IsActive = true;

        // Start objective's timeAllotted timer
        if (CooldownSystem.IsOnCooldown(Id))
        {
            CooldownSystem.RefreshCooldown(Id);
            Debug.Log("REFRESH COOLDOWN FOR " + gameObject);
        }
                
        else
        {
            CooldownSystem.PutOnCooldown(this);
            Debug.Log("START COOLDOWN FOR " + gameObject);
        }
        
        MapObjectiveManager.Instance.ObjectiveWasActivated(this);
            
        // Objective will no longer expire once activated
        _mapObjectiveExpire.RemoveExpireTimeOnActivate();
    }

    protected virtual void OnObjectiveOutOfTime()
    {
        Debug.Log("Timer ENDED for " + gameObject + "!");
        IsActive = false;
        MapObjectiveManager.Instance.StartNewObjectiveAfterEnded(this);
    }
    

    private void ObjectiveOutOfTime(int cooldownId)
    {
        if (Id == cooldownId)
        {
            OnObjectiveOutOfTime();
        }
           
    }

    public abstract string GetObjectiveDescription();

    ///-///////////////////////////////////////////////////////////
    /// Place this objective at one of its potential spawn locations while avoiding
    /// a previously used location.
    /// 
    private void PlaceObjectiveAtRandomLocation()
    {
        if (potentialSpawnLocations.Count > 1 && _previousSpawnLocation != null)
        {
            while (_currentSpawnLocation == _previousSpawnLocation)
            {
                int randomLocationIndex = Random.Range(0, potentialSpawnLocations.Count);

                _currentSpawnLocation = potentialSpawnLocations[randomLocationIndex];
            }
        }
        else
        {
            int randomLocationIndex = Random.Range(0, potentialSpawnLocations.Count);

            _currentSpawnLocation = potentialSpawnLocations[randomLocationIndex];
        }
        

        _previousSpawnLocation = _currentSpawnLocation;

        transform.position = _currentSpawnLocation.position;
    }

    ///-///////////////////////////////////////////////////////////
    /// Increase amount of xp and money given for completing an objective by a certain percentage.
    /// 
    public void IncreaseRewards(float percentage)
    {
        completionReward.xpAmount = (int) (completionReward.xpAmount + (completionReward.xpAmount * percentage));
        
        completionReward.moneyAmount = (int) (completionReward.moneyAmount + (completionReward.moneyAmount * percentage));
    }

    public float GetTimeRemaining()
    {
        return CooldownSystem.GetRemainingDuration(Id);
    }

    public int Id { get; set; }
    
    public float CooldownDuration { get; set; }
}
