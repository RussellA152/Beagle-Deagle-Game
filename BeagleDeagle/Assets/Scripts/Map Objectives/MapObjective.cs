using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] private LayerMask layersThatCanStart;
    protected Transform PlayerTransform;
    
    // This timer will begin when the player is too far away from the objective (will fail when it reaches 0)
    [SerializeField, Range(1f, 180f)] 
    private float timeToFailWhenFarAway;

    [Header("Range")]
    [SerializeField, Range(1f, 100f)] 
    private float startingRange = 10f;
    
    [SerializeField, Range(1f, 100f)] 
    private float exitingRange = 15f;
    
    [Header("Reward")]
    // Xp and money to give to player on objective completion (increases every minute)
    [SerializeField] private CurrencyReward completionReward;
    
    // The actual reward the player will receive on objective completion (won't scale while doing objective to avoid exploit)
    private CurrencyReward _actualCompletionReward;
    
    private CooldownSystem _cooldownSystem;
    private MapObjectiveExpire _mapObjectiveExpire;

    // Range indicator will display one of the ranges of a map objective (ex. Soul Collect range, survival range, etc.)
    protected RangeIndicator RangeIndicator;

    // Is this objective currently being played?
    public bool IsActive { get; private set; }
    
    protected bool PlayerInsideStartingArea = false;
    protected bool PlayerInsideExitArea = false;

    private void Awake()
    {
        RangeIndicator = GetComponentInChildren<RangeIndicator>();
        
        OnObjectiveAwake();
    }

    private void Start()
    {
        _cooldownSystem.OnCooldownEnded += ObjectiveOutOfTime;
        
        // Hide range until player activates this map objective
        RangeIndicator.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnObjectiveEnable();

        // Reward to be given is updated in OnEnable
        _actualCompletionReward = completionReward;
    }

    private void OnDisable()
    {
        OnObjectiveDisable();
        
    }

    private void OnDestroy()
    {
        _cooldownSystem.OnCooldownEnded -= ObjectiveOutOfTime;
    }

    private void Update()
    {
        OnObjectiveUpdate();
    }
    

    protected virtual void OnObjectiveAwake()
    {
        Id = 50;
        _cooldownSystem = GetComponent<CooldownSystem>();

        CooldownDuration = timeToFailWhenFarAway;

        _mapObjectiveExpire = GetComponent<MapObjectiveExpire>();
    }

    protected virtual void OnObjectiveEnable()
    {
        PlaceObjectiveAtRandomLocation();
    }

    protected virtual void OnObjectiveDisable()
    {
        PlayerInsideStartingArea = false;
        IsActive = false;
    }

    protected virtual void OnObjectiveUpdate()
    {
        bool playerInsideStartingAreaNow = Physics2D.OverlapCircle(transform.position, startingRange, layersThatCanStart);

        if (!PlayerInsideStartingArea && playerInsideStartingAreaNow)
        {
            PlayerInsideStartingArea = true;
            OnObjectiveEnter();
        }
        else if (PlayerInsideStartingArea && !playerInsideStartingAreaNow)
        {
            PlayerInsideStartingArea = false;
        }

        bool playerInsideExitAreaNow = Physics2D.OverlapCircle(transform.position, exitingRange, layersThatCanStart);

        if (!PlayerInsideExitArea && playerInsideExitAreaNow)
        {
            PlayerInsideExitArea = true;
            
            // Stop counting down if player has re-entered the starting range
            if (_cooldownSystem.IsOnCooldown(Id))
            {
                Debug.Log("Stop exit timer!");
                MapObjectiveManager.Instance.PlayerReturnedToObjective(this);
                _cooldownSystem.RemoveCooldown(Id);
            }
            
        }
        else if (PlayerInsideExitArea && !playerInsideExitAreaNow)
        {
            PlayerInsideExitArea = false;
            OnObjectiveExit();
        }
        
    }

    ///-///////////////////////////////////////////////////////////
    /// When completing the objective, give the player a reward.
    /// Then do something abstract afterwards (ex. destroy objects that are no longer needed)
    /// 
    protected virtual void OnObjectiveCompletion()
    {
        // Give reward
        currencyEvents.InvokeGiveXp(_actualCompletionReward.xpAmount);
        
        MapObjectiveManager.Instance.StartNewObjectiveAfterEnded(this);

    }

    // All objectives will remove their cooldown on completion except for the DefendObjective.
    // This is because the DefendObjective must run out of time to be considered completed, which results
    // in the cooldown being removed anyways.
    protected void RemoveCooldown()
    {
        if(_cooldownSystem.IsOnCooldown(Id))
            _cooldownSystem.RemoveCooldown(Id);
    }

    
    protected virtual void OnObjectiveEnter()
    {
        // If the objective is already active, don't try to activate again
        if (!IsActive)
        {
            MapObjectiveManager.Instance.ObjectiveWasActivated(this);
            
            // Objective will no longer expire once activated
            _mapObjectiveExpire.RemoveExpireTimeOnActivate();
            
            IsActive = true;
            
        }

        DisplayRangeIndicator();
        
        
    }

    private void OnObjectiveExit()
    {
        if (!IsActive) return;
        
        // If player is too far away from objective, then start counting down
        if (_cooldownSystem.IsOnCooldown(Id))
        {
            _cooldownSystem.RefreshCooldown(Id);
        }
                
        else
        {
            _cooldownSystem.PutOnCooldown(this);
            Debug.Log("START COOLDOWN FOR " + gameObject);
        }
        MapObjectiveManager.Instance.ObjectiveWasExited(this);
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
        // * The actual reward the player will receive only updates in OnEnable * 
        completionReward.xpAmount = (int) (completionReward.xpAmount + (completionReward.xpAmount * percentage));
        
        completionReward.moneyAmount = (int) (completionReward.moneyAmount + (completionReward.moneyAmount * percentage));
    }
    
    protected virtual void DisplayRangeIndicator()
    {
        // Display the exiting range of this map objective
        RangeIndicator.gameObject.SetActive(true);
        
        RangeIndicator.SetSize(new Vector2(startingRange, startingRange));
        
    }
    

    public void ReceivePlayerReference(Transform pTransform)
    {
        PlayerTransform = pTransform;
    }

    ///-///////////////////////////////////////////////////////////
    /// Return the amount of time remaining until failure, after the player has walked far away from the objective.
    /// 
    public float GetExitTimeRemaining()
    {
        return _cooldownSystem.GetRemainingDuration(Id);
    }
    

    public int Id { get; set; }
    
    public float CooldownDuration { get; set; }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, startingRange);
        
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, exitingRange);
    }
}
