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

    private void OnEnable()
    {
        CurrentInput.Instance.OnPlayerChangedController += ChangeButtonImageSprite;
    }

    private void OnDisable()
    {
        CurrentInput.Instance.OnPlayerChangedController -= ChangeButtonImageSprite;
    }

    private void ChangeButtonImageSprite(CurrentInput.ControllerType controllerType)
    {
        switch (controllerType)
        {
            case CurrentInput.ControllerType.Keyboard:
                _imageToChange.sprite = keyboardSprite;
                break;
            case CurrentInput.ControllerType.Xbox:
                _imageToChange.sprite = xboxSprite;
                break;
            case CurrentInput.ControllerType.Playstation:
                _imageToChange.sprite = playstationSprite;
                break;
        }
    }
    
}
