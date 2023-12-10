using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private Button quitButton;

    private void Awake()
    {
        quitButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        quitButton.onClick.AddListener(CloseGame);
    }

    private void OnDisable()
    {
        quitButton.onClick.RemoveListener(CloseGame);
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}
