using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GameOverScreen : MonoBehaviour
{
    // Will tell us that the gamer timer concluded
    [SerializeField] private GameEvents gameEvents;
    // Will tell us that the player has died
    [SerializeField] private PlayerEvents playerEvents;
    
    // Will pause the game for us
    [SerializeField] private GamePauser gamePauser;

    // All objects to turn on when the victory screen appears
    [SerializeField] private GameObject[] gameOverGameObjects;

    [SerializeField] private TMP_Text menuTitle;

    // The button to be selected when the victory screen appears
    [SerializeField] private GameObject continueButtonGameObject;

    [SerializeField, Range(1f, 5f)] private float delayUntilGameOver;

    private void Start()
    {
        foreach (GameObject gameObj in gameOverGameObjects)
        {
            gameObj.SetActive(false);
        }
    }

    private void OnEnable()
    {
        playerEvents.onPlayerDied += DeathScreen;
        gameEvents.onGameTimeConcluded += VictoryScreen;
    }

    private void OnDisable()
    {
        playerEvents.onPlayerDied -= DeathScreen;
        gameEvents.onGameTimeConcluded -= VictoryScreen;
    }
    

    private void DeathScreen()
    {
        menuTitle.text = "You Died!";

        StartCoroutine(DelayUntilGameOverScreen());
    }

    private void VictoryScreen()
    {
        menuTitle.text = "You Win!";
        
        ShowScreen();
    }

    private IEnumerator DelayUntilGameOverScreen()
    {
        yield return new WaitForSeconds(delayUntilGameOver);
        ShowScreen();
    }

    ///-///////////////////////////////////////////////////////////
    /// When the game's timer concludes (ex. 15 minutes), pause the game
    /// and show the victory screen to the player.
    /// 
    private void ShowScreen()
    {
        EventSystem.current.SetSelectedGameObject(continueButtonGameObject);
        
        foreach (GameObject gameObj in gameOverGameObjects)
        {
            gameObj.SetActive(true);
        }
        
        gamePauser.PauseGameAutomatically();
    }
}
