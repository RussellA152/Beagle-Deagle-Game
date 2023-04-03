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
    private GunWeapon weapon;

    [SerializeField]
    private TextMeshProUGUI ammoMagText;

    [SerializeField]
    private TextMeshProUGUI maxAmmoText;

    private void Update()
    {
        ammoMagText.text = weapon.bulletsLoaded.ToString();
        maxAmmoText.text = weapon.ammoInReserve.ToString();
    }



}
