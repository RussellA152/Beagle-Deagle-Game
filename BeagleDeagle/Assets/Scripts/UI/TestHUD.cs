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
    
    [SerializeField] private TextMeshProUGUI maxAmmoText;
    
    [SerializeField] private TextMeshProUGUI waveMessageText;
    
    [SerializeField] private TextMeshProUGUI currentHealthText;
    
    [SerializeField] private TextMeshProUGUI maxHealthText;
    
    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent += UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged += UpdateOnPlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged += UpdateOnPlayerMaxHealthText;

        playerEvents.onPlayerBulletsLoadedChanged += UpdateAmmoText;


    }

    private void OnDisable()
    {
        wavesBegan.changeHUDTextEvent -= UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged -= UpdateOnPlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged -= UpdateOnPlayerMaxHealthText;

        playerEvents.onPlayerBulletsLoadedChanged -= UpdateAmmoText;
        
        
    }
    
    public void UpdateOnPlayerCurrentHealthText(float currentHealth)
    {
        currentHealthText.text = currentHealth.ToString();
    }

    public void UpdateOnPlayerMaxHealthText(float maxHealth)
    {
        maxHealthText.text = maxHealth.ToString();
    }

    public void UpdateWaveMessageText(string newText)
    {
        waveMessageText.text = newText;
    }
    
    public void UpdateAmmoText(int bulletsLoaded)
    {
        ammoMagText.text = bulletsLoaded.ToString();
    }

}
