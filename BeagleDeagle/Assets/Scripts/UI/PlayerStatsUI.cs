using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

///-///////////////////////////////////////////////////////////
/// Display the current health, weapon, ammo, and abilities of the player.
/// 
public class PlayerStatsUI : MonoBehaviour
{
    [Header("Event Systems")]
    [SerializeField]
    private WaveBeginEvents wavesBegan;

    [SerializeField]
    private PlayerEvents playerEvents;

    // Needed to know basic stats and values specific to each character (xp needed to rank up)
    //private PlayerData _playerData;

    [SerializeField] private TextMeshProUGUI waveMessageText;

    [Header("Health UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    
    [Header("Ammo UI")]
    [SerializeField] private TextMeshProUGUI currentAmmoMagText;
    [SerializeField] private TextMeshProUGUI maxAmmoMagText;

    [Header("Xp UI")] 
    [SerializeField] private Image xpImageFill;
    [SerializeField] private TMP_Text levelText;

    [Header("Utility UI")]
    [SerializeField] private Image utilityImage;
    [SerializeField] private Image utilityImageFill;
    
    [Header("Ultimate UI")]
    [SerializeField] private Image ultimateImage;
    [SerializeField] private Image ultimateImageFill;

    [Header("Weapon Information UI")] 
    [SerializeField] private Image weaponImage;
    [SerializeField] private RectTransform bulletPanel;
    [SerializeField] private GameObject bulletDisplayPrefab;
    private int _bulletImageCount = 100;
    private List<Image> _bulletImages = new List<Image>();
    
    // HUD remembers the value of the max health and max ammo magazine size
    private float _playerMaxHealth;
    private int _maxAmmoCount;
    
    private void Awake()
    {
        // Allocate 100 bullet images to use ( * may need to increase if we have weapons that have more than 100 bullets)
        for (int i = 0; i < _bulletImageCount; i++)
        {
            GameObject bulletDisplay = Instantiate(bulletDisplayPrefab, bulletPanel.transform, false);
            
            bulletDisplay.SetActive(false);

            _bulletImages.Add(bulletDisplay.GetComponent<Image>());
        }
    }
    
    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent += UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged += UpdatePlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged += UpdatePlayerMaxHealthText;

        playerEvents.getPlayerXpNeededUntilLevelUp += UpdateXpImage;
        playerEvents.onPlayerLeveledUp += UpdateCurrentLevelText;

        playerEvents.onPlayerObtainedNewUltimate += UpdateUltimateImage;
        playerEvents.onPlayerObtainedNewUtility += UpdateUtilityImage;

        playerEvents.onPlayerSwitchedWeapon += AddBulletsToHUD;
        playerEvents.onPlayerBulletsLoadedChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        wavesBegan.changeHUDTextEvent -= UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged -= UpdatePlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged -= UpdatePlayerMaxHealthText;
        
        playerEvents.getPlayerXpNeededUntilLevelUp -= UpdateXpImage;
        playerEvents.onPlayerLeveledUp -= UpdateCurrentLevelText;
        
        playerEvents.onPlayerObtainedNewUltimate -= UpdateUltimateImage;
        playerEvents.onPlayerObtainedNewUtility -= UpdateUtilityImage;
        
        playerEvents.onPlayerBulletsLoadedChanged -= UpdateAmmoText;
        playerEvents.onPlayerSwitchedWeapon -= AddBulletsToHUD;
    }

    // private void GetPlayerStats(PlayerData data)
    // {
    //     _playerData = data;
    // }

    private void UpdatePlayerCurrentHealthText(float currentHealth)
    {
        healthBar.fillAmount = currentHealth / _playerMaxHealth;
        currentHealthText.text = currentHealth.ToString();
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
        currentAmmoMagText.text = bulletsLoaded.ToString();
        
        int bulletDifference = bulletsLoaded - _maxAmmoCount;
        
        if (_bulletImages.Count > 0f)
        {
            // If the player lost ammo, disable a bullet from the UI
            if (bulletDifference < 0)
            {
                _bulletImages[bulletsLoaded].gameObject.SetActive(false);
            }
            else
            {
                // If the player gained ammo, display a bullet on the UI
                for (int i = Mathf.Abs(bulletDifference); i < _maxAmmoCount; i++)
                {
                    _bulletImages[i].gameObject.SetActive(true);
                }
            }
        }
    }

    private void UpdateXpImage(float xpNeededLeft)
    {
        xpImageFill.fillAmount = xpNeededLeft;
    }

    private void UpdateCurrentLevelText(int newLevel)
    {
        levelText.text = "LVL " + newLevel.ToString();
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
        // Player received new weapon, so update the maximum magazine size text and variable
        _maxAmmoCount = gunData.magazineSize;
        maxAmmoMagText.text = gunData.magazineSize.ToString();

        // Change the weapon image to use the sprite of the player's new gun
        weaponImage.sprite = gunData.gunEffectsData.weaponSprite;
        
        Sprite bulletSprite = gunData.bulletPrefab.GetComponent<SpriteRenderer>().sprite;

        // Change all bullet images to have a new sprite of the current gun's bullet
        for (int i = 0; i < _bulletImages.Count; i++)
        {
            _bulletImages[i].sprite = bulletSprite;
            
            if (i < _maxAmmoCount)
            {
                _bulletImages[i].gameObject.SetActive(true);
            }
            else
            {
                _bulletImages[i].gameObject.SetActive(false);
            }


        }
        
    }

}
