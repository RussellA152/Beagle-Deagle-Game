using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenModeButton : MonoBehaviour
{
    //[SerializeField] private Button fullScreenButton;
    [SerializeField] private Button borderlessButton;
    [SerializeField] private Button windowedButton;
    
    private void OnEnable()
    {
        //fullScreenButton.onClick.AddListener(SetFullScreen);
        
        borderlessButton.onClick.AddListener(SetBorderlessWindowed);
        windowedButton.onClick.AddListener(SetWindowed);
    }

    private void OnDisable()
    {
        //fullScreenButton.onClick.RemoveListener(SetFullScreen);
        
        borderlessButton.onClick.RemoveListener(SetBorderlessWindowed);
        windowedButton.onClick.RemoveListener(SetWindowed);
    }

    private void SetFullScreen()
    {
        Debug.Log("Set FullScreenMode to FullScreen!");
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    private void SetBorderlessWindowed()
    {
        Debug.Log("Set FullScreenMode to Borderless Windowed!");
        Screen.fullScreenMode = FullScreenMode.Windowed;
        // Set the window size and position to cover the entire screen
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }

    private void SetWindowed()
    {
        Debug.Log("Set FullScreenMode to Windowed!");
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

}
