using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
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

    [SerializeField] private TextMeshProUGUI waveMessageText;

    [Header("Health UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    
    [Header("Ammo UI")]
    [SerializeField] private TextMeshProUGUI ammoMagText;

    [Header("Utility UI")]
    [SerializeField] private Image utilityImage;
    [SerializeField] private Image utilityImageFill;
    
    [Header("Ultimate UI")]
    [SerializeField] private Image ultimateImage;
    [SerializeField] private Image ultimateImageFill;

    [SerializeField] private RectTransform bulletPanel;
    [SerializeField] private GameObject bulletTESTSprite;
    private List<Image> _bulletImages = new List<Image>();
    
    private float _playerMaxHealth;
    private int _currentAmmoCount;
    
    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent += UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged += UpdatePlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged += UpdatePlayerMaxHealthText;

        playerEvents.onPlayerObtainedNewUltimate += UpdateUltimateImage;
        playerEvents.onPlayerObtainedNewUtility += UpdateUtilityImage;

        playerEvents.onPlayerBulletsLoadedChanged += UpdateAmmoText;

        playerEvents.onPlayerSwitchedWeapon += AddBulletsToHUD;
    }

    private void OnDisable()
    {
        wavesBegan.changeHUDTextEvent -= UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged -= UpdatePlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged -= UpdatePlayerMaxHealthText;
        
        playerEvents.onPlayerObtainedNewUltimate -= UpdateUltimateImage;
        playerEvents.onPlayerObtainedNewUtility -= UpdateUtilityImage;
        
        playerEvents.onPlayerBulletsLoadedChanged -= UpdateAmmoText;
        playerEvents.onPlayerSwitchedWeapon -= AddBulletsToHUD;
    }
    
    private void UpdatePlayerCurrentHealthText(float currentHealth)
    {
        healthBar.fillAmount = currentHealth / _playerMaxHealth;
        currentHealthText.text = currentHealth.ToString() + " /";
    }

    private void UpdatePlayerMaxHealthText(float maxHealth)
    {
        _playerMaxHealth = maxHealth;
        maxHealthText.text = maxHealth.ToString();
    }
    

    private void UpdateWaveMessageText(string newText)
    {
        waveMessageText.text = newText;
    }
    
    private void UpdateAmmoText(int bulletsLoaded)
    {
        ammoMagText.text = bulletsLoaded.ToString();
        _currentAmmoCount = bulletsLoaded;
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

    private void AddBulletsToHUD(GunData gunData)
    {
        for (int i = 0; i < gunData.magazineSize; i++)
        {
            Debug.Log("Make a bullet image!");
            // var createImage = Instantiate(bulletTESTSprite);
            // bulletTESTSprite.GetComponent<Image>().sprite = 
            // createImage.transform.SetParent(bulletPanel.transform, false);
        }
    }

}
