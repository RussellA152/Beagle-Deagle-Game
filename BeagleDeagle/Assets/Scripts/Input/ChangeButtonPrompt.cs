using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonPrompt : MonoBehaviour
{
    private Image _imageToChange;
    [Header("Button Images Depending on Current Controller")]
    [SerializeField] private Sprite keyboardSprite;
    [SerializeField] private Sprite xboxSprite;
    [SerializeField] private Sprite playstationSprite;

    private void Awake()
    {
        _imageToChange = GetComponent<Image>();
    }
    

    private void Start()
    {
        // Subscribing at start because OnEnable() gets a null reference
        PlayerControllerTypeManager.Instance.OnPlayerChangedController += ChangeButtonImageSprite;
    }
    
    private void OnDestroy()
    {
        PlayerControllerTypeManager.Instance.OnPlayerChangedController -= ChangeButtonImageSprite;
    }

    private void ChangeButtonImageSprite(PlayerControllerTypeManager.ControllerType controllerType)
    {
        switch (controllerType)
        {
            case PlayerControllerTypeManager.ControllerType.Keyboard:
                _imageToChange.sprite = keyboardSprite;
                break;
            case PlayerControllerTypeManager.ControllerType.Xbox:
                _imageToChange.sprite = xboxSprite;
                break;
            case PlayerControllerTypeManager.ControllerType.Playstation:
                _imageToChange.sprite = playstationSprite;
                break;
        }
    }
    
}
