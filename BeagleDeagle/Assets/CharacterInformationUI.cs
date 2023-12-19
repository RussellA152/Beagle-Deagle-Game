using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterInformationUI : MonoBehaviour
{
    [SerializeField] private Button[] allCharacterButtons;
    
    [Header("Weapon Information")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponText;
    
    [Header("Passive Ability Information")]
    [SerializeField] private Image passiveAbilityImage;
    [SerializeField] private TMP_Text passiveAbilityText;
    
    [Header("Utility Ability Information")]
    [SerializeField] private Image utilityAbilityImage;
    [SerializeField] private TMP_Text utilityAbilityText;
    
    [Header("Ultimate Ability Information")]
    [SerializeField] private Image ultimateAbilityImage;
    [SerializeField] private TMP_Text ultimateAbilityText;

    private void Start()
    {
        // Listen for when any character select button is highlighted by the player
        foreach (Button button in allCharacterButtons)
        {
            button.GetComponent<SelectButtonOnHighlight>().onButtonSelected += UpdateAllTextAndImages;
        }
    }
    
    private void OnDestroy()
    {
        foreach (Button button in allCharacterButtons)
        {
            button.GetComponent<SelectButtonOnHighlight>().onButtonSelected -= UpdateAllTextAndImages;
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Display text and images about the weapon and abilities of the character that the player is currently viewing.
    /// By default, information about Deagle Beagle is displayed first.
    /// 
    public void UpdateAllTextAndImages(Button buttonSelected)
    {
        CharacterSelectButton characterSelectButton = buttonSelected.GetComponent<CharacterSelectButton>();
        PlayerData playerData = characterSelectButton.dataToDisplay;

        // Update information about this character's weapon
        GunData playerGunData = playerData.gunData;
        weaponText.text = playerGunData.gunName + ": " + playerGunData.GetDescription();
        weaponImage.sprite = playerGunData.gunEffectsData.weaponSprite;

        // Update information about this character's passive ability
        PassiveAbilityData passiveAbilityData = playerData.passiveAbilityData;
        passiveAbilityText.text = passiveAbilityData.passiveName + ": " +  passiveAbilityData.GetDescription();
        passiveAbilityImage.sprite = passiveAbilityData.abilityIcon;

        // Update information about this character's utility ability
        UtilityAbilityData utilityAbilityData = playerData.utilityAbilityData;
        utilityAbilityText.text = utilityAbilityData.abilityName + ": " +  utilityAbilityData.GetDescription();
        utilityAbilityImage.sprite = utilityAbilityData.abilitySprite;

        // Update information about this character's ultimate ability
        UltimateAbilityData ultimateAbilityData = playerData.ultimateAbilityData;
        ultimateAbilityText.text = ultimateAbilityData.abilityName + ": " +  ultimateAbilityData.GetDescription();
        ultimateAbilityImage.sprite = ultimateAbilityData.abilitySprite;
    }
}
