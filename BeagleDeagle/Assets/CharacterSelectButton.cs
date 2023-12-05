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
    private Image buttonImage;
    private Button buttonComponent;
    
    public GameObject selectableCharacter;

    [SerializeField] private GameObject otherCharacterSelectButton;
    [SerializeField] private GameObject levelChoiceButton;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        buttonComponent = GetComponent<Button>();
    }

    private void Start()
    {
        PlayerCharacterSpawner.Instance.onPlayerChoseCharacter += DisableOnCharacterChoice;
    }

    private void OnEnable()
    {
        buttonComponent.onClick.AddListener(ChooseCharacter);
        buttonComponent.onClick.AddListener(DisableOtherButtons);
    }

    private void OnDisable()
    {
        buttonComponent.onClick.RemoveListener(ChooseCharacter);
        PlayerCharacterSpawner.Instance.onPlayerChoseCharacter -= DisableOnCharacterChoice;
    }

    private void ChooseCharacter()
    {
        PlayerCharacterSpawner.Instance.InvokePlayerChoseCharacter(selectableCharacter);
    }

    private void DisableOtherButtons()
    {
        buttonImage.enabled = false;
        buttonComponent.interactable = false;
        levelChoiceButton.SetActive(true);
        gameObject.SetActive(false);
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
