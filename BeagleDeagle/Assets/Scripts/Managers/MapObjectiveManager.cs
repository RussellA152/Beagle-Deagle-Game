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
    [SerializeField] 
    private List<MapObjective> potentialMapObjectives = new List<MapObjective>();

    [SerializeField] private MapObjective currentMapObjective;
    private MapObjective _previousMapObjective;

    [SerializeField, Range(5f, 180f)] 
    private float timeBetweenObjectives = 5f;

    // Manager will need a cooldown to start another objective
    private CooldownSystem _cooldownSystem;

    private void Awake()
    {
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
    }

    ///-///////////////////////////////////////////////////////////
    /// Every x seconds, pick a random objective to set as the current objective that
    /// the player can complete.
    /// 
    private void StartNewObjective(int cooldownId)
    {
        if (Id == cooldownId)
        {
            if (currentMapObjective != null)
            {
                Debug.Log("REMOVE PREVIOUS OBJECTIVE");
                currentMapObjective.gameObject.SetActive(false);
            }
            
            Debug.Log("START RANDOM OBJECTIVE");

            currentMapObjective = PickRandomObjective();
            
            _cooldownSystem.PutOnCooldown(this);
        }
        
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

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
