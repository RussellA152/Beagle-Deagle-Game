using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private SoundEvents soundEvents;
    [SerializeField] private UISounds uiSounds;

    private Button _button;
    private SelectButtonOnHighlight _selectButtonOnHighlight;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _selectButtonOnHighlight = GetComponent<SelectButtonOnHighlight>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        //_selectButtonOnHighlight.onButtonSelected += OnSelect;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
        //_selectButtonOnHighlight.onButtonSelected -= OnSelect;
    }

    private void OnClick()
    {
        soundEvents.InvokeUISoundPlay(uiSounds.clickSound, uiSounds.volume);
    }

    // private void OnSelect(Button button)
    // {
    //     soundEvents.InvokeUISoundPlay(uiSounds.selectSound, uiSounds.volume);
    // }
    
}
