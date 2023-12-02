
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    
    public float loadTimer = 1f;

    public int currentScene;
    public event Action onLevelChosen;

    public event Action onGameSceneLoaded;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnGameSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnGameSceneLoaded;
    }

    IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(loadTimer);
        LoadNewScene();

    }

    public void InvokeLevelChosen(int sceneChoice)
    {
        currentScene = sceneChoice;
        onLevelChosen?.Invoke();
        
        Debug.Log($"Scene {sceneChoice} was chosen. Will now load...");
        StartCoroutine(SceneDelay());
    }

    private void LoadNewScene()
    { 
        SceneManager.LoadScene(currentScene, LoadSceneMode.Additive);

    }
    
    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // TODO: Make this properly check a scene type!
        
        // This method will be called when a new scene is loaded
        Debug.Log("Scene loaded: " + scene.name);
        
        onGameSceneLoaded?.Invoke();

    }
}
