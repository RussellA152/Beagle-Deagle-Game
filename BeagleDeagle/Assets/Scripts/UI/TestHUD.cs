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
    private WaveBeginEventSO wavesBegan;

    [SerializeField]
    private PlayerEventSO playerEvents;

    [Header("Text Fields")]
    [SerializeField]
    private TextMeshProUGUI ammoMagText;

    [SerializeField]
    private TextMeshProUGUI maxAmmoText;

    [SerializeField]
    private TextMeshProUGUI waveMessageText;

    [SerializeField]
    private TextMeshProUGUI currentHealthText;

    [SerializeField]
    private TextMeshProUGUI maxHealthText;

    [SerializeField]
    private TextMeshProUGUI utilityNameText;

    [SerializeField]
    private TextMeshProUGUI utilityUsesText;

    [SerializeField]
    private TextMeshProUGUI ultimateNameText;

    [SerializeField]
    private TextMeshProUGUI ultimateCooldownText;


    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent += UpdateHudText;

        playerEvents.currentHealthChangedEvent += UpdateCurrentHealthText;

        playerEvents.maxHealthChangedEvent += UpdateMaxHealthText;

        playerEvents.playerUtilityUsesUpdatedEvent += UpdateUtilityUsesText;

        playerEvents.playerUtilityNameChangeEvent += UpdateUtilityNameText;

        playerEvents.playerBulletsLoadedChangedEvent += UpdateAmmoText;

        playerEvents.playerUltimateCooldownEvent += UpdateUltimateCooldownText;

        playerEvents.playerUltimateNameChangeEvent += UpdateUltimateNameText;

    }

    private void OnDisable()
    {
        wavesBegan.changeHUDTextEvent -= UpdateHudText;

        playerEvents.currentHealthChangedEvent -= UpdateCurrentHealthText;

        playerEvents.maxHealthChangedEvent -= UpdateMaxHealthText;

        playerEvents.playerUtilityUsesUpdatedEvent -= UpdateUtilityUsesText;

        playerEvents.playerUtilityNameChangeEvent -= UpdateUtilityNameText;

        playerEvents.playerBulletsLoadedChangedEvent -= UpdateAmmoText;

        playerEvents.playerUltimateCooldownEvent -= UpdateUltimateCooldownText;

        playerEvents.playerUltimateNameChangeEvent -= UpdateUltimateNameText;
    }

    public void UpdateCurrentHealthText(float currentHealth)
    {
        currentHealthText.text = currentHealth.ToString();
    }

    public void UpdateMaxHealthText(float maxHealth)
    {
        maxHealthText.text = maxHealth.ToString();
    }

    public void UpdateHudText(string newText)
    {
        waveMessageText.text = newText;
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

    public void UpdateUltimateCooldownText(float time)
    {
        ultimateCooldownText.text = ((int) time).ToString();
    }

    public void UpdateUltimateNameText(string name)
    {
        Debug.Log("Name is: " + name);
        ultimateNameText.text = name;
    }

}
