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
    [SerializeField] private TextMeshProUGUI currentAmmoMagText;
    [SerializeField] private TextMeshProUGUI maxAmmoMagText;

    [Header("Utility UI")]
    [SerializeField] private Image utilityImage;
    [SerializeField] private Image utilityImageFill;
    
    [Header("Ultimate UI")]
    [SerializeField] private Image ultimateImage;
    [SerializeField] private Image ultimateImageFill;

    [Header("Bullet Stack UI")]
    [SerializeField] private RectTransform bulletPanel;
    [SerializeField] private GameObject bulletDisplayPrefab;
    private List<Image> _bulletImages = new List<Image>();
    
    // HUD remembers the value of the max health and max ammo magazine size
    private float _playerMaxHealth;
    private int _maxAmmoCount;
    
    private void Awake()
    {
        // Allocate 100 bullet images to use ( * may need to increase if we have weapons that have more than 100 bullets)
        for (int i = 0; i < 100; i++)
        {
            GameObject bulletDisplay = Instantiate(bulletDisplayPrefab);
            
            bulletDisplay.SetActive(false);

            bulletDisplay.transform.SetParent(bulletPanel.transform, false);
            
            _bulletImages.Add(bulletDisplay.GetComponent<Image>());
        }
    }
    
    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent += UpdateWaveMessageText;

        playerEvents.onPlayerCurrentHealthChanged += UpdatePlayerCurrentHealthText;

        playerEvents.onPlayerMaxHealthChanged += UpdatePlayerMaxHealthText;

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
        
        playerEvents.onPlayerObtainedNewUltimate -= UpdateUltimateImage;
        playerEvents.onPlayerObtainedNewUtility -= UpdateUtilityImage;
        
        playerEvents.onPlayerBulletsLoadedChanged -= UpdateAmmoText;
        playerEvents.onPlayerSwitchedWeapon -= AddBulletsToHUD;
    }

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
        Debug.Log("HUD AMMO: " + bulletsLoaded);
        currentAmmoMagText.text = bulletsLoaded.ToString();

        //int bulletDifference = _maxAmmoCount - bulletsLoaded;
        int bulletDifference = bulletsLoaded - _maxAmmoCount;
        
        if (_bulletImages.Count > 0f)
        {
            if (bulletDifference < 0)
            {
                _bulletImages[bulletsLoaded].gameObject.SetActive(false);
            }
            else
            {
                for (int i = Mathf.Abs(bulletDifference); i < _maxAmmoCount; i++)
                {
                    _bulletImages[i].gameObject.SetActive(true);
                }
            }
        }
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
        Debug.Log("WEAPON WAS CHANGED!");
        _maxAmmoCount = gunData.magazineSize;
        
        // Player received new weapon, so update the maximum magazine size text
        maxAmmoMagText.text = gunData.magazineSize.ToString();
        
        Sprite bulletSprite = gunData.bulletType.bulletPrefab.GetComponent<SpriteRenderer>().sprite;

        // Change all bullet images to have a new sprite of the current gun's bullet
        for (int i = 0; i < _bulletImages.Count; i++)
        {
            _bulletImages[i].GetComponent<Image>().sprite = bulletSprite;
            
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
