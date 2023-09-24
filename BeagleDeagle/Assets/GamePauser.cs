using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class GamePauser : MonoBehaviour
{
    [SerializeField] private GameEvents gameEvents;

    [SerializeField] private PlayerEvents playerEvents;
    
    private PlayerInput _playerInput;

    // Is the game already paused?
    private bool _pauseDelayed;
    private bool _gamePaused;

    // When the game is paused, put a small delay before the game can be unpaused
    private float _gameResumeDelay = 0.5f;


    [Header("Pause Menu Objects")]
    [SerializeField] private GameObject pauseMenuGameObject;
    
    [SerializeField] private Button resumeButton;

    private void Awake()
    {
        // Hide the pause menu on awake
        pauseMenuGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += FindPlayerInput;
        
        resumeButton.onClick.AddListener(ResumeButton);
    }

    private void OnDisable()
    {
        playerEvents.givePlayerGameObject -= FindPlayerInput;
        
        resumeButton.onClick.RemoveListener(ResumeButton);
    }

    // Get the player's PlayerInput component to get their "Pause" binding
    private void FindPlayerInput(GameObject playerGameObject)
    {
        _playerInput = playerGameObject.GetComponent<PlayerInput>();

        // Pausing and resuming the game both use "Pause" binding
        _playerInput.currentActionMap.FindAction("Pause").performed += PauseManually;
        _playerInput.currentActionMap.FindAction("Pause").performed += ResumeManually;
    }

    private void PauseManually(CallbackContext context)
    {
        // Pause the game only if the game is not already paused
        if (_gamePaused) return;
        
        pauseMenuGameObject.SetActive(true);
        
        PauseGame();
    }

    private void ResumeManually(CallbackContext context)
    {
        ResumeButton();
    }
    
    private void ResumeButton()
    {
        // Unpause game only if it was already paused
        // and if the pause delay is not ongoing
        if (!_gamePaused || _pauseDelayed) return;
        
        // Hide pause menu
        pauseMenuGameObject.SetActive(false);
        
        ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        _gamePaused = true;

        // Begin pause delay after pausing the game
        StartCoroutine(GamePauseDelay());

        // Tell all listeners that the game was paused
        gameEvents.InvokeOnGamePauseEvent();
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
        
        _gamePaused = false;

        // Tell all listeners that the game was unpaused
        gameEvents.InvokeOnGameResumeEvent();
    }
}
