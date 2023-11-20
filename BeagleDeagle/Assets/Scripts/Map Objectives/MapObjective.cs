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
    [SerializeField] private GameEvents gameEvents;

    [SerializeField] private List<Transform> potentialSpawnLocations = new List<Transform>();

    private Transform _currentSpawnLocation;
    private Transform _previousSpawnLocation;
    
    // How long will the player play this objective? (ex. Survive for "duration" seconds)
    [SerializeField, Range(1f, 180f)] 
    private float timeAllotted;

    [SerializeField] private CurrencyReward completionReward;

    protected CooldownSystem CooldownSystem;
    private MapObjectiveExpire _mapObjectiveExpire;

    // Collider that player needs to touch to begin objective
    private CapsuleCollider2D _startObjectiveCollider;
    
    // Is this objective currently being played?
    public bool IsActive { get; private set; }

    private void Awake()
    {
        Id = 50;
        CooldownSystem = GetComponent<CooldownSystem>();
        CooldownDuration = timeAllotted;

        _mapObjectiveExpire = GetComponent<MapObjectiveExpire>();
        _startObjectiveCollider = GetComponent<CapsuleCollider2D>();

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
        // Re-activate start collider
        _startObjectiveCollider.enabled = true;

        OnObjectiveDisable();
    }

    private void OnDestroy()
    {
        CooldownSystem.OnCooldownEnded -= ObjectiveOutOfTime;
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
        gameEvents.InvokeMapObjectiveCompletedEvent(completionReward);
        
        MapObjectiveManager.instance.StartNewObjectiveAfterEnded(this);
        
        CooldownSystem.RemoveCooldown(Id);
        
    }

    private void ObjectiveOutOfTime(int cooldownId)
    {
        if (Id == cooldownId)
        {
            Debug.Log("Timer ENDED for " + gameObject + "!");
            IsActive = false;
            MapObjectiveManager.instance.StartNewObjectiveAfterEnded(this);
        }
           
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsActive = true;

            // Start objective when player enters 
            Debug.Log("Start objective timer!");

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
                
            MapObjectiveManager.instance.ObjectiveWasActivated();
            
            _mapObjectiveExpire.RemoveExpireTimeOnActivate();

            // Disable start collider when player enters
            _startObjectiveCollider.enabled = false;
        }
        
    }

    public int Id { get; set; }
    
    public float CooldownDuration { get; set; }
}
