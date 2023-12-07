
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public int currentScene;
    public event Action onLevelChosen;
    public event Action onGameSceneLoaded;
    
    public event Action onMenuSceneLoaded;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        //SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void InvokeLevelChosen(int sceneChoice)
    {
        currentScene = sceneChoice;
        onLevelChosen?.Invoke();
        
        Debug.Log($"Scene {sceneChoice} was chosen. Will now load...");
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        SceneManager.LoadScene(currentScene, LoadSceneMode.Single);

    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This method will be called when a new scene is loaded
        Debug.Log("Scene loaded: " + scene.name);

        if (scene.name.Contains("Game"))
        {
            Debug.Log("Scene is a game level");
            onGameSceneLoaded?.Invoke();
        }
        else if (scene.name.Contains("Menu"))
        {
            Debug.Log("Scene is a menu.");
            onMenuSceneLoaded?.Invoke();
        }

    }
}
