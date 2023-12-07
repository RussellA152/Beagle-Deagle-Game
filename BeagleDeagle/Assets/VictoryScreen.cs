using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class VictoryScreen : MonoBehaviour
{
    // Will tell us that the gamer timer concluded
    [SerializeField] private GameEvents gameEvents;

    // Will pause the game for us
    [SerializeField] private GamePauser gamePauser;

    // All objects to turn on when the victory screen appears
    [SerializeField] private GameObject[] victoryScreenGameObjects;

    // The button to be selected when the victory screen appears
    [SerializeField] private GameObject continueButtonGameObject;

    private void Start()
    {
        foreach (GameObject gameObj in victoryScreenGameObjects)
        {
            gameObj.SetActive(false);
        }
    }

    private void OnEnable()
    {
        gameEvents.onGameTimeConcluded += ShowVictoryScreen;
    }

    private void OnDisable()
    {
        gameEvents.onGameTimeConcluded -= ShowVictoryScreen;
    }

    ///-///////////////////////////////////////////////////////////
    /// When the game's timer concludes (ex. 15 minutes), pause the game
    /// and show the victory screen to the player.
    /// 
    private void ShowVictoryScreen()
    {
        EventSystem.current.SetSelectedGameObject(continueButtonGameObject);
        
        foreach (GameObject gameObj in victoryScreenGameObjects)
        {
            gameObj.SetActive(true);
        }
        
        gamePauser.PauseGameAutomatically();
    }
    
}
