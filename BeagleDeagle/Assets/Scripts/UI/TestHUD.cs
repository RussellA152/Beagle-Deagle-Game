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
    
    [SerializeField] private TextMeshProUGUI rollCooldownText;
    
    [SerializeField] private TextMeshProUGUI utilityNameText;
    
    [SerializeField] private TextMeshProUGUI utilityUsesText;
    
    [SerializeField] private TextMeshProUGUI ultimateNameText;
    
    [SerializeField] private TextMeshProUGUI ultimateCooldownText;


    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent += UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged += UpdateOnPlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged += UpdateOnPlayerMaxHealthText;

        playerEvents.onPlayerRollStartsCooldown += UpdateRollStartsCooldownCooldownText;

        playerEvents.onPlayerUtilityUsesUpdated += UpdateUtilityUsesText;

        playerEvents.onPlayerUtilityNameChanged += UpdateUtilityNameText;

        playerEvents.onPlayerBulletsLoadedChanged += UpdateAmmoText;

        playerEvents.onPlayerUltimateAbilityCooldown += UpdateUltimateAbilityCooldownText;

        playerEvents.onPlayerUltimateAbilityNameChanged += UpdateUltimateAbilityNameText;

    }

    private void OnDisable()
    {
        wavesBegan.changeHUDTextEvent -= UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged -= UpdateOnPlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged -= UpdateOnPlayerMaxHealthText;

        playerEvents.onPlayerUtilityUsesUpdated -= UpdateUtilityUsesText;

        playerEvents.onPlayerUtilityNameChanged -= UpdateUtilityNameText;

        playerEvents.onPlayerBulletsLoadedChanged -= UpdateAmmoText;

        playerEvents.onPlayerUltimateAbilityCooldown -= UpdateUltimateAbilityCooldownText;

        playerEvents.onPlayerUltimateAbilityNameChanged -= UpdateUltimateAbilityNameText;
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

    public void UpdateRollStartsCooldownCooldownText(float rollCooldownTime)
    {
        rollCooldownText.text = ((int) rollCooldownTime).ToString() + " seconds";
    }

    public void UpdateAmmoText(int bulletsLoaded)
    {
        ammoMagText.text = bulletsLoaded.ToString();
    }

    public void UpdateUtilityUsesText(int uses)
    {
        utilityUsesText.text = uses.ToString();
    }

    public void UpdateUtilityNameText(string name)
    {
        utilityNameText.text = name;
    }

    public void UpdateUltimateAbilityCooldownText(float timeLeft)
    {
        ultimateCooldownText.text = ((int) timeLeft).ToString() + " seconds";
    }

    public void UpdateUltimateAbilityNameText(string name)
    {
        Debug.Log("Name is: " + name);
        ultimateNameText.text = name;
    }

}
