using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We can use ScriptableObjects for data about our weapons.
// This is useful for our weapon upgrades since we can specify new damage or ammo values for the upgraded version
[CreateAssetMenu(fileName = "NewGun", menuName = "ScriptableObjects/GunData/Automatic")]
public class GunData : ScriptableObject
{
    [Range(0, 1000)]
    public float damagePerHit;

    [Header("Fire Rate (Bullets Per Second)")]
    public float fireRate; // The number of bullets fired per second

    [Header("Ammo")]
    public int magazineSize; // how much ammo can this weapon hold for its total magazine?

    //public int maxAmmoInReserve; // how much ammo can this weapon hold for total capacity?
    public float totalReloadTime; // how long will this gun take to reload to full?

    [Header("Bullet Logic")]
    public Bullet bullet; // what bullet is spawned when shooting?

    public float spreadX; // spread of bullet in X direction
    public float spreadY; // spread of bullet in Y direction

    [Header("Penetration")]
    [Range(1f, 50f)]
    public int penetrationCount; // how many enemies can this gun's bullet pass through?

}
