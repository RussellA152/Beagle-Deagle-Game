using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private Button _quitButton;

    private void Awake()
    {
        _quitButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _quitButton.onClick.AddListener(CloseGame);
    }

    private void OnDisable()
    {
        _quitButton.onClick.RemoveListener(CloseGame);
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}
