using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class GunData : ScriptableObject
{
    // Name displayed in-game
    public string gunName;
    
    public Sprite sprite;

    [Header("Fire Rate (Bullets Per Second)")]
    [Range(0f, 100f)]
    public float fireRate; // The number of bullets fired per second

    [Header("Ammo")]
    [Range(0, 100)]
    public int magazineSize; // how much ammo can this weapon hold for its total magazine?
    
    [Header("Reloading")]
    [Range(0f, 30f)]
    public float totalReloadTime;

    // What kind of bullet will this gun shoot (ex. regular, fire, radiation... etc.)
    public BulletTypeData bulletType;
    
    [Header("Weapon Spread")]
    [Range(0f, 20f)]
    public float bulletSpread; // spread of bullet in X direction
    
    [Header("Penetration")]
    [Range(1f, 100f)]
    public int penetrationCount; // how many enemies can this gun's bullet pass through?
    
    //[HideInInspector]
    //public Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)
    
    ///-///////////////////////////////////////////////////////////
    /// Return the damage of this weapon.
    /// 
    public abstract float GetDamage();
    
}
