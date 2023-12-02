using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///-///////////////////////////////////////////////////////////
/// A button that will ask which playable character the player wants to use in the game level.
/// 
public class CharacterSelectButton : MonoBehaviour
{
    [HideInInspector]
    public Button buttonComponent;
    
    public GameObject selectableCharacter;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();
    }

    private void Start()
    {
        StartMenuManager.Instance.onPlayerChoseCharacter += DisableOnCharacterChoice;
    }

    private void OnEnable()
    {
        buttonComponent.onClick.AddListener(ChooseCharacter);
    }

    private void OnDisable()
    {
        buttonComponent.onClick.RemoveListener(ChooseCharacter);
        StartMenuManager.Instance.onPlayerChoseCharacter -= DisableOnCharacterChoice;
    }

    private void ChooseCharacter()
    {
        StartMenuManager.Instance.InvokePlayerChoseCharacter(selectableCharacter);
    }

    ///-///////////////////////////////////////////////////////////
    /// When a character has been chosen, turn off the button component so player
    /// cannot make a second choice.
    /// 
    private void DisableOnCharacterChoice()
    {
        buttonComponent.enabled = false;
    }
}
