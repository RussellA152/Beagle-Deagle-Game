using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance;

    public GameObject characterChosen;

    public int sceneChosen;

    public event Action onPlayerChoseCharacter;

    public event Action onLevelChosen; 

    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
    }
    
    public void InvokePlayerChoseCharacter(GameObject characterChoice)
    {
        characterChosen = characterChoice;
        onPlayerChoseCharacter?.Invoke();
    }

    public void InvokeLevelChosen(int sceneChoice)
    {
        sceneChosen = sceneChoice;
        onLevelChosen?.Invoke();
    }
}
