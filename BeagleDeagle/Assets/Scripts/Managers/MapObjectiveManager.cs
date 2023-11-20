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
    public static MapObjectiveManager instance;
    
    [SerializeField] 
    private List<MapObjective> potentialMapObjectives = new List<MapObjective>();

    [SerializeField] private MapObjective currentMapObjective;
    private MapObjective _previousMapObjective;

    [SerializeField, Range(5f, 180f)] 
    private float timeBetweenObjectives = 5f;

    // Manager will need a cooldown to start another objective
    public CooldownSystem CooldownSystem {get; private set;}

    private void Awake()
    {
        instance = this;
        
        // Setting cooldown Id and cooldown duration
        Id = 30;
        CooldownDuration = timeBetweenObjectives;
        
        CooldownSystem = GetComponent<CooldownSystem>();

        // Pick a random objective when cooldown ends
        CooldownSystem.OnCooldownEnded += StartNewObjective;
        

    }

    private void Start()
    {
        // Start a cooldown for the first objective to start
        CooldownSystem.PutOnCooldown(this);

        // Disable all map objectives at the start of the game
        foreach (MapObjective mapObjective in potentialMapObjectives)
        {
            mapObjective.gameObject.SetActive(false);
        }
        
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

    public void ObjectiveWasActivated()
    {
        CooldownSystem.RemoveCooldown(Id);
    }
    

    public void StartNewObjectiveAfterEnded(MapObjective mapObjective)
    {
        mapObjective.gameObject.SetActive(false);

        CooldownSystem.PutOnCooldown(this);
    }
    
    public void StartNewObjectiveAfterExpired(MapObjectiveExpire mapObjectiveExpire)
    {
        mapObjectiveExpire.gameObject.SetActive(false);

        CooldownSystem.PutOnCooldown(this);
    }

    public float GetNextObjectiveTime()
    {
        return CooldownSystem.GetRemainingDuration(Id);
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
