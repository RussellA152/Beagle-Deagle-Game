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
    
    [RestrictedPrefab(typeof(PlayerController))]
    public GameObject selectableCharacter;

    public PlayerData dataToDisplay;
    
    private void Awake()
    {
        _buttonComponent = GetComponent<Button>();
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
    
}
