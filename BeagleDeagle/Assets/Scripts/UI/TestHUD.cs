using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// TEMPORARY. I AM USING THIS ONLY TO SEE/DEBUG THE AMMO OF THE PLAYER'S GUN AT ALL TIMES
/// </summary>
public class TestHUD : MonoBehaviour
{
    [Header("Event Systems")]
    [SerializeField]
    private WaveBeginEvents wavesBegan;

    [SerializeField]
    private PlayerEvents playerEvents;

    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI ammoMagText;

    [SerializeField] private TextMeshProUGUI waveMessageText;

    [SerializeField] private Image healthBar;
    //[SerializeField] private TextMeshProUGUI currentHealthText;
    
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [SerializeField] private Image utilityImage;
    [SerializeField] private Image utilityImageFill;
    
    [SerializeField] private Image ultimateImage;
    [SerializeField] private Image ultimateImageFill;
    
    
    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent += UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged += UpdateOnPlayerCurrentHealthText;

        //playerEvents.onPlayerMaxHealthChanged

        playerEvents.onPlayerObtainedNewUltimate += UpdateUltimateImage;
        playerEvents.onPlayerObtainedNewUtility += UpdateUtilityImage;

        playerEvents.onPlayerBulletsLoadedChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        wavesBegan.changeHUDTextEvent -= UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged -= UpdateOnPlayerCurrentHealthText;

        //playerEvents.onPlayerMaxHealthChanged -= UpdateOnPlayerMaxHealthText;
        
        playerEvents.onPlayerObtainedNewUltimate -= UpdateUltimateImage;
        playerEvents.onPlayerObtainedNewUtility -= UpdateUtilityImage;
        
        playerEvents.onPlayerBulletsLoadedChanged -= UpdateAmmoText;
    }
    
    private void UpdateOnPlayerCurrentHealthText(float currentHealth)
    {
        //healthBar.fillAmount = currentHealth / ;
        //currentHealthText.text = currentHealth.ToString();
    }
    

    private void UpdateWaveMessageText(string newText)
    {
        waveMessageText.text = newText;
    }
    
    private void UpdateAmmoText(int bulletsLoaded)
    {
        ammoMagText.text = bulletsLoaded.ToString();
    }

    private void UpdateUtilityImage(UtilityAbilityData utility)
    {
        utilityImage.sprite = utility.abilitySprite;
        utilityImageFill.sprite = utility.abilitySprite;
    }
    
    private void UpdateUltimateImage(UltimateAbilityData ultimate)
    {
        ultimateImage.sprite = ultimate.abilitySprite;
        ultimateImageFill.sprite = ultimate.abilitySprite;
    }

}
