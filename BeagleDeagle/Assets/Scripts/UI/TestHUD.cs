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
    [SerializeField]
    private WaveBeginEventSO wavesBegan;

    [SerializeField]
    private PlayerEventSO playerEvents;

    [SerializeField]
    private GunData weaponData;

    [SerializeField]
    private UtilityAbilityData utilityData;

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


    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent += UpdateHudText;

        playerEvents.currentHealthChangedEvent += UpdateCurrentHealthText;

        playerEvents.maxHealthChangedEvent += UpdateMaxHealthText;

        playerEvents.playerUtilityUsesUpdatedEvent += UpdateUtilityUsesText;
    }

    private void OnDisable()
    {
        wavesBegan.changeHUDTextEvent -= UpdateHudText;

        playerEvents.currentHealthChangedEvent -= UpdateCurrentHealthText;

        playerEvents.maxHealthChangedEvent -= UpdateMaxHealthText;
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

    public void UpdateAmmoText(string newText)
    {
        ammoMagText.text = newText;
    }

    public void UpdateUtilityUsesText(int uses)
    {
        utilityUsesText.text = uses.ToString();
    }

    private void Update()
    {
        //ammoMagText.text = weapon.bulletsLoaded.ToString();
        ammoMagText.text = weaponData.bulletsLoaded.ToString();
        utilityNameText.text = utilityData.name;

        //utilityUsesText.text = utilityData.uses.ToString();
        //maxAmmoText.text = weapon.ammoInReserve.ToString();
    }



}
