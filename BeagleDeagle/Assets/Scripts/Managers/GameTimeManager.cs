using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;
    
    [Range(1f, 1500f)]
    public float targetGameDuration = 1200f; // 20 minutes in seconds
    public float ElapsedTime { get; private set; }
    
    private bool _gameConcludedFromTime;

    public event Action onGameTimeConcluded;

    private void Awake()
    {
        // Create singleton instance
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        _gameConcludedFromTime = false;
        
    }

    private void Update()
    {
        // Check if the game still has time left
        if (ElapsedTime >= targetGameDuration && !_gameConcludedFromTime)
        {
            Debug.Log("Game Time ENDED!");
            _gameConcludedFromTime = true;
            
            onGameTimeConcluded?.Invoke();
        }
        else
        {
            ElapsedTime += Time.deltaTime;
        }
    }
}
