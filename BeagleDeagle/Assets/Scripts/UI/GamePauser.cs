using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

public class GamePauser : MonoBehaviour
{
    [SerializeField] private GameEvents gameEvents;

    [SerializeField] private PlayerEvents playerEvents;
    
    private PlayerInput _playerInput;

    // Is the game already paused?
    private bool _pauseDelayed;
    private bool _gamePausedManually;
    private bool _gamePausedAutomatically;

    // When the game is paused, put a small delay before the game can be unpaused
    private float _gameResumeDelay = 0.5f;

    [SerializeField] private GameObject[] pauseMenuObjects;
    
    [SerializeField] private Button resumeButton;

    private void Start()
    {
        // Hide all pause menu gameObjects at the start
        foreach (GameObject pauseMenuGameObject in pauseMenuObjects)
        {
            pauseMenuGameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += FindPlayerInput;
        
        resumeButton.onClick.AddListener(ResumeButton);
    }

    private void OnDisable()
    {
        ResumeGame();
        
        playerEvents.givePlayerGameObject -= FindPlayerInput;
        
        resumeButton.onClick.RemoveListener(ResumeButton);
        
    }

    // Get the player's PlayerInput component to get their "Pause" binding
    private void FindPlayerInput(GameObject playerGameObject)
    {
        Debug.Log("Hello player: " + playerGameObject);
        _playerInput = playerGameObject.GetComponent<PlayerInput>();

        // Pausing and resuming the game both use "Pause" binding
        _playerInput.currentActionMap.FindAction("Pause").performed += PauseManually;
        _playerInput.currentActionMap.FindAction("Pause").performed += ResumeManually;
        
    }

    private void PauseManually(CallbackContext context)
    {
        // Pause the game only if the game is not already paused
        if (_gamePausedManually || _gamePausedAutomatically) return;
        
        _gamePausedManually = true;

        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        

        foreach (GameObject pauseMenuGameObject in pauseMenuObjects)
        {
            pauseMenuGameObject.SetActive(true);
        }
        

        PauseGame();
    }

    public void PauseGameAutomatically()
    {
        // Don't pause if already paused
        if (_gamePausedManually || _gamePausedAutomatically) return;

        _gamePausedAutomatically = true;
        
        PauseGame();
    }

    private void ResumeManually(CallbackContext context)
    {
        // Only allow resume if the game was paused by the player's manual input
        if (_gamePausedAutomatically) return;
        
        ResumeButton();
    }
    
    private void ResumeButton()
    {
        // Unpause if the pause delay is not ongoing
        if (_pauseDelayed) return;
        
        // Hide pause menu
        foreach (GameObject pauseMenuGameObject in pauseMenuObjects)
        {
            pauseMenuGameObject.SetActive(false);
        }
        
        ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;

        // Begin pause delay after pausing the game
        StartCoroutine(GamePauseDelay());

        // Tell all listeners that the game was paused
        gameEvents.InvokeGamePauseEvent();
    }

    // Small delay before allowing game to be resumed after being paused
    private IEnumerator GamePauseDelay()
    {
        _pauseDelayed = true;
        
        yield return new WaitForSecondsRealtime(_gameResumeDelay);
        
        _pauseDelayed = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        _gamePausedManually = false;
        _gamePausedAutomatically = false;
        
        // Tell all listeners that the game was unpaused
        gameEvents.InvokeGameResumeEvent();
    }
}
