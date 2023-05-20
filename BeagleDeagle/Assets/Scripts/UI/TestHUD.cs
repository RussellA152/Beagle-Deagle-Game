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
    private GunWeapon<GunData> weapon;

    [SerializeField]
    private TextMeshProUGUI ammoMagText;

    [SerializeField]
    private TextMeshProUGUI maxAmmoText;

    [SerializeField]
    private TextMeshProUGUI waveMessageText;

    private void OnEnable()
    {
        wavesBegan.changeHUDTextEvent.AddListener(UpdateHudText);

        //wavesBegan.changeHUDTextEvent.AddListener(UpdateAmmoText);
    }

    public void UpdateHudText(string newText)
    {
        waveMessageText.text = newText;
    }

    public void UpdateAmmoText(string newText)
    {
        ammoMagText.text = newText;
    }

    private void Update()
    {
        ammoMagText.text = weapon.bulletsLoaded.ToString();
        //maxAmmoText.text = weapon.ammoInReserve.ToString();
    }



}
