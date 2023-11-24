using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

///-///////////////////////////////////////////////////////////
/// Map Objective Manager is responsible for selecting and removing objectives in the game level every X minutes.
/// A cooldown will begin after an objective has been completed or has expired to start a new objective.
/// 
public class MapObjectiveManager : MonoBehaviour, IHasCooldown
{
    public static MapObjectiveManager Instance;

    [SerializeField] private GameEvents gameEvents;
    
    [SerializeField] 
    private List<MapObjective> potentialMapObjectives = new List<MapObjective>();

    [SerializeField] private MapObjective currentMapObjective;
    private MapObjective _previousMapObjective;

    [SerializeField, Range(5f, 180f)] 
    private float timeBetweenObjectives = 5f;

    [SerializeField, Range(0.1f, 1f)] 
    private float rewardScalePercentage;
    // Manager will need a cooldown to start another objective
    private CooldownSystem _cooldownSystem;

    private void Awake()
    {
        Instance = this;
        
        // Setting cooldown Id and cooldown duration
        Id = 30;
        CooldownDuration = timeBetweenObjectives;
        
        _cooldownSystem = GetComponent<CooldownSystem>();

        // Pick a random objective when cooldown ends
        _cooldownSystem.OnCooldownEnded += StartNewObjective;
        

    }

    private void Start()
    {
        // Start a cooldown for the first objective to start
        _cooldownSystem.PutOnCooldown(this);

        // Disable all map objectives at the start of the game
        foreach (MapObjective mapObjective in potentialMapObjectives)
        {
            mapObjective.gameObject.SetActive(false);
        }
        
    }

    private void OnEnable()
    {
        gameEvents.onGameMinutePassed += RewardScaleWithTime;
    }

    private void OnDisable()
    {
        gameEvents.onGameMinutePassed -= RewardScaleWithTime;
    }

    ///-///////////////////////////////////////////////////////////
    /// Every x seconds, pick a random objective to set as the current objective that
    /// the player can complete.
    /// 
    private void StartNewObjective(int cooldownId)
    {
        if (Id != cooldownId) return;

        if (currentMapObjective == null)
        {
            currentMapObjective = PickRandomObjective();
        }
        else if (!currentMapObjective.IsActive)
        {
            currentMapObjective.gameObject.SetActive(false);
            currentMapObjective = PickRandomObjective();
        }

        //CooldownSystem.PutOnCooldown(this);
    }

    private MapObjective PickRandomObjective()
    {
        int randomIndex = 0; 
        
        if (potentialMapObjectives.Count > 1 && _previousMapObjective != null)
        {
            while (currentMapObjective == _previousMapObjective)
            {
                // Find a random objective
                randomIndex = Random.Range(0, potentialMapObjectives.Count);
                
                currentMapObjective = potentialMapObjectives[randomIndex];
            }
        }
        else
        {
            // Find a random objective
            randomIndex = Random.Range(0, potentialMapObjectives.Count);
            
            currentMapObjective = potentialMapObjectives[randomIndex];

        }
        
        currentMapObjective.gameObject.SetActive(true);
        
        _previousMapObjective = currentMapObjective;

        return currentMapObjective;
    }

    public void ObjectiveWasActivated(MapObjective mapObjective)
    {
        _cooldownSystem.RemoveCooldown(Id);
        
        gameEvents.InvokeMapObjectiveBeginEvent(mapObjective);
    }

    public void ObjectiveWasEntered(MapObjective mapObjective)
    {
        gameEvents.InvokePlayerEnteredMapObjective(mapObjective);
    }
    
    public void ObjectiveWasExited(MapObjective mapObjective)
    {
        gameEvents.InvokePlayerExitedMapObjective(mapObjective);
    }
    
    public void StartNewObjectiveAfterEnded(MapObjective mapObjective)
    {
        mapObjective.gameObject.SetActive(false);

        _cooldownSystem.PutOnCooldown(this);
        
        gameEvents.InvokeMapObjectiveEndedEvent(mapObjective);
    }
    
    public void StartNewObjectiveAfterExpired(MapObjectiveExpire mapObjectiveExpire)
    {
        mapObjectiveExpire.gameObject.SetActive(false);

        _cooldownSystem.PutOnCooldown(this);
    }

    ///-///////////////////////////////////////////////////////////
    /// Every minute, make all map objectives increase their rewards.
    /// This makes it so completing map objectives in later stages don't feel worthless.
    /// 
    private void RewardScaleWithTime()
    {
        foreach (MapObjective mapObjective in potentialMapObjectives)
        {
            mapObjective.IncreaseRewards(rewardScalePercentage);
        }
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
