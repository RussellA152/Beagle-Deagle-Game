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
    // private Image buttonImage;
    private Button _buttonComponent;
    
    public GameObject selectableCharacter;
    

    private void Awake()
    {
        _buttonComponent = GetComponent<Button>();
    }

    private void Start()
    {
        PlayerCharacterSpawner.Instance.onPlayerChoseCharacter += DisableOnCharacterChoice;
    }

    private void OnDestroy()
    {
        PlayerCharacterSpawner.Instance.onPlayerChoseCharacter -= DisableOnCharacterChoice;
    }

    private void OnEnable()
    {
        _buttonComponent.onClick.AddListener(ChooseCharacter);
    }

    private void OnDisable()
    {
        _buttonComponent.onClick.RemoveListener(ChooseCharacter);
    }

    private void ChooseCharacter()
    {
        PlayerCharacterSpawner.Instance.InvokePlayerChoseCharacter(selectableCharacter);
    }
    

    ///-///////////////////////////////////////////////////////////
    /// When a character has been chosen, turn off the button component so player
    /// cannot make a second choice.
    /// 
    private void DisableOnCharacterChoice()
    {
        _buttonComponent.enabled = false;
        gameObject.SetActive(false);
    }
}
