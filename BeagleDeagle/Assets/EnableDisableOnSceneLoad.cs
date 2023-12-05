using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableOnSceneLoad : MonoBehaviour
{
    [SerializeField] private EnableType enableType;
    [SerializeField] private DisableType disableType;

    [SerializeField] private GameObject[] gameObjectsAffected;
    
    private enum EnableType
    {
        Menu,
        
        Game
    }
    
    private enum DisableType
    {
        Menu,
        
        Game
    }

    private void Start()
    {
        if (enableType == EnableType.Menu)
        {
            SceneLoader.Instance.onGameSceneLoaded += EnableGameObjects;
        }
        else if (enableType ==  EnableType.Game)
        {
            SceneLoader.Instance.onGameSceneLoaded += EnableGameObjects;
        }
        
        if (disableType == DisableType.Menu)
        {
            SceneLoader.Instance.onMenuSceneLoaded += DisableGameObjects;
        }
        else if (disableType ==  DisableType.Game)
        {
            SceneLoader.Instance.onGameSceneLoaded += DisableGameObjects;
        }
    }

    private void DisableGameObjects()
    {
        foreach (GameObject gameObj in gameObjectsAffected)
        {
            gameObj.SetActive(false);
        }
    }
    
    private void EnableGameObjects()
    {
        foreach (GameObject gameObj in gameObjectsAffected)
        {
            gameObj.SetActive(true);
        }
    }
}
