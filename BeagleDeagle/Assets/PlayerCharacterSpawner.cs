using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacterSpawner : MonoBehaviour
{
    public static PlayerCharacterSpawner Instance;
    
    public GameObject currentCharacter;

    public event Action onPlayerChoseCharacter;
    
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

    private void Start()
    {
        SceneLoader.Instance.onGameSceneLoaded += SpawnPlayerInGameScene;
    }

    private void OnDisable()
    {
        SceneLoader.Instance.onGameSceneLoaded -= SpawnPlayerInGameScene;
    }

    public void InvokePlayerChoseCharacter(GameObject characterChoice)
    {
        currentCharacter = characterChoice;
        onPlayerChoseCharacter?.Invoke();
    }

    private void SpawnPlayerInGameScene()
    {
        Instantiate(currentCharacter);
    }
}
