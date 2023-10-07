using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class GunData : ScriptableObject
{
    // Name displayed in-game
    public string gunName;

    public GunEffectsData gunEffectsData;
    
    [Header("Fire Rate (Bullets Per Second)")]
    [Range(0f, 100f)]
    public float fireRate; // The number of bullets fired per second

    [Header("Ammo")]
    [Range(0, 100)]
    public int magazineSize; // how much ammo can this weapon hold for its total magazine?
    
    [Header("Reloading")]
    [Range(0f, 30f)]
    public float totalReloadTime;

    [Header("Bullet Data")] 
    [RestrictedPrefab(typeof(IBulletUpdatable))]
    // What bullet will this gun shoot?
    public GameObject bulletPrefab;
    // What data does this bullet use?
    public BulletData bulletData;
    // What are the status effects of this bullet? (* PREFAB MUST BE COMPATIBLE * )
    public StatusEffectTypes statusEffects;
    // Where should bullets spawn from (barrel)
    public Vector2 bulletSpawnLocation;
    
    [Header("Weapon Spread")]
    [Range(0f, 20f)]
    public float bulletSpread; // spread of bullet in X direction
    
    [Header("Penetration")]
    [Range(1f, 100f)]
    public int penetrationCount; // how many enemies can this gun's bullet pass through?
    
    ///-///////////////////////////////////////////////////////////
    /// Return the damage of this weapon.
    /// 
    public abstract float GetDamage();
    
}
