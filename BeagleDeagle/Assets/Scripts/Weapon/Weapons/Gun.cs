using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour, IGunDataUpdatable, IDamager
{
    [SerializeField] 
    private Transform bulletSpawnPoint; // Where does this bullet get shot from? (i.e the barrel)
    
    [Header("Required Components")]
    [SerializeField] private PlayerEvents playerEvents;
    [SerializeField] private GunData weaponData;
    private SpriteRenderer _spriteRenderer;

    private float _shootInput; // Input for shooting
    
    public bool ActuallyShooting { get; private set; } // is the player shooting (i.e, not idle or reloading or just moving)
    
    private int _bulletsShot; // how much ammo has the player shot since the last reload or refill?
    
    private int _bulletsLoaded; // how much ammo is currently in the magazine?

    private bool _isReloading;

    private bool _canShoot;
    private bool _canReload;

    private float _lastTimeShot;
    private float _timeElapsedSinceShot;

    private int _bulletPoolKey;

    [Header("Modifiers")]
    [SerializeField, NonReorderable]
    private List<DamageModifier> damageModifiers = new List<DamageModifier>(); // A bonus percentage applied to the gun's damage
    [SerializeField, NonReorderable]
    private List<PenetrationModifier> penetrationModifiers = new List<PenetrationModifier>();
    [SerializeField, NonReorderable]
    private List<SpreadModifier> spreadModifiers = new List<SpreadModifier>();
    [SerializeField, NonReorderable]
    private List<AttackSpeedModifier> fireRateModifiers = new List<AttackSpeedModifier>();
    [SerializeField, NonReorderable]
    private List<ReloadSpeedModifier> reloadSpeedModifiers = new List<ReloadSpeedModifier>();
    [SerializeField, NonReorderable]
    private List<AmmoLoadModifier> ammoLoadModifiers = new List<AmmoLoadModifier>();

    private float _bonusDamage = 1f;
    private int _bonusPenetration = 0;
    private float _bonusSpread = 1f;
    private float _bonusFireRate = 1f;
    private float _bonusReloadSpeed = 1f;
    private float _bonusAmmoLoad = 1f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerEvents.InvokeUpdateAmmoLoadedText(Mathf.RoundToInt(_bulletsLoaded * _bonusAmmoLoad));

        _canReload = true;
        
        _lastTimeShot = 0f;
        _timeElapsedSinceShot = 0f;

        _bulletsShot = 0;
        
        _bulletsLoaded = weaponData.magazineSize;
        
        _bulletPoolKey = weaponData.bulletType.bulletPrefab.GetComponent<IPoolable>().PoolKey;
    }

    private void OnEnable()
    {
        playerEvents.onPlayerSwitchedWeapon += UpdateScriptableObject;

        playerEvents.InvokeNewWeaponEvent(weaponData);

    }

    private void Update()
    {
        //_lastTimeShot += Time.deltaTime;
        _timeElapsedSinceShot += Time.deltaTime;

        // If the player has no ammo loaded into their weapon, begin reloading
        // If the gun is not already reloading, begin a coroutine for the reload
        if (_bulletsLoaded <= 0f && !_isReloading && _canReload)
        {
            StartCoroutine(Reload());
            return;
        }

        // If player is holding down "fire" button, then attempt to shoot
        // Also check that the gun's fire rate is ready to shoot
        if (_shootInput > 0 && CheckAmmo() && CheckIfCanFire())
        {
            Attack();
            ActuallyShooting = true;
        }
        else
        {
            ActuallyShooting = false;
        }
    }
    
    // Update the GunData scriptable object to a new one.
    // This can change many stats like damage, penetration, fireRate, appearance (sprite), and more.
    public void UpdateScriptableObject(GunData scriptableObject)
    {
        //weaponData.bulletSpawnPoint = bulletSpawnPoint;

        weaponData = scriptableObject;
        
        RefillAmmoCompletely();

        _bulletsLoaded = Mathf.RoundToInt(_bulletsLoaded * _bonusAmmoLoad);

        _spriteRenderer.sprite = weaponData.sprite;

        // After swapping to new weapon, show the ammo on the HUD
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
    }

    ///-///////////////////////////////////////////////////////////
    /// Return the current gun data of this weapon
    /// 
    public GunData GetCurrentData()
    {
        return weaponData;
    }
    

    #region Shooting
    
    public void OnFire(CallbackContext context)
    {
        // Allow shoot input, if the player is allowed to shoot (usually disabled upon death)
        if (!_canShoot) return;
        
        _shootInput = context.ReadValue<float>();
    }
    
    public bool CheckIfCanFire()
    {
        return Time.time - _lastTimeShot > 1f / weaponData.fireRate * _bonusFireRate;
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Fetch a bullet from object pooler, then shoot it out
    /// 
    public void Attack()
    {
        // The bullet will spawn at the barrel of the gun
        //weaponData.bulletSpawnPoint = bulletSpawnPoint;
        
        // Fetch a bullet from object pooler
        GameObject newBullet = ObjectPooler.instance.GetPooledObject(_bulletPoolKey);

        if (newBullet != null)
        {
            // Tell BulletType to update the bullet with the data it needs
            // For example, give fire damage to the bullet, or give life steal values to the bullet
            // Pass in the bullet gameObject and the player gameObject(to retrieve modifiers)
            IBulletUpdatable projectile = weaponData.bulletType.UpdateBulletWithData(newBullet, transform.parent.parent.gameObject);
            
            // Pass in the damage and penetration values of this gun, to the bullet being shot
            // Also account for any modifications to the gun damage and penetration (e.g, an item purchased by trader that increases player gun damage)
            projectile.UpdateDamageAndPenetrationValues(weaponData.GetDamage() * _bonusDamage, weaponData.penetrationCount + _bonusPenetration);
            

            // Set the position to be at the barrel of the gun
            newBullet.transform.position = bulletSpawnPoint.position;

            // Apply the spread to the bullet's rotation
            newBullet.transform.rotation = CalculateWeaponSpread();

            newBullet.gameObject.SetActive(true);

            _bulletsShot++;

            _bulletsLoaded--;
        }
        

        // Player has shot gun, so reset timeElapsedSinceShot to 0 seconds
        _timeElapsedSinceShot = 0f;
        _lastTimeShot = Time.time;

        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Add a random offset to the bullet's Y position
    /// to simulate random spread.
    public virtual Quaternion CalculateWeaponSpread()
    {
        // Calculate the spread angle
        float spreadAngle = Random.Range(-weaponData.bulletSpread * _bonusSpread, weaponData.bulletSpread * _bonusSpread);

        return Quaternion.Euler(0f, 0f, spreadAngle) * bulletSpawnPoint.rotation;
    }

    public void SetCanShoot(bool boolean)
    {
        _canShoot = boolean;
    }
    
    public float ReturnLastTimeShot()
    {
        return _timeElapsedSinceShot;
    }

    #endregion

    #region Reloading

    // Call reload function when the player presses the reload key
    public void OnReload(CallbackContext context)
    {
        if(!_isReloading && _canReload)
            StartCoroutine(Reload());
    }
    
    public IEnumerator Reload()
    {
        _isReloading = true;
        ActuallyShooting = false;
        
        // Wait until reload is finished
        yield return new WaitForSeconds(weaponData.totalReloadTime * _bonusReloadSpeed);
        
        RefillAmmoCompletely();

        _isReloading = false;

        // Then call event that ammo has changed
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
    }
    public bool CheckAmmo()
    {
        // If player has no ammo in reserve,
        // then force them to reload by returning false
        return _bulletsLoaded > 0f;
    }
    
    ///-///////////////////////////////////////////////////////////
    /// When the gun is done reloading, refill all the ammo
    /// 
    private void RefillAmmoCompletely()
    {
        _bulletsLoaded = weaponData.magazineSize;
        _bulletsShot = 0;
    }
    
    public void SetCanReload(bool boolean)
    {
        _canReload = boolean;
    }
    #endregion

    #region StatModifiers
    public void AddDamageModifier(DamageModifier modifierToAdd)
    {
        damageModifiers.Add(modifierToAdd);
        _bonusDamage += (_bonusDamage * modifierToAdd.bonusDamage);

    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        damageModifiers.Remove(modifierToRemove);
        _bonusDamage /= (1 + modifierToRemove.bonusDamage);
    }


    public void AddPenetrationModifier(PenetrationModifier modifierToAdd)
    {
        penetrationModifiers.Add(modifierToAdd);
        _bonusPenetration += modifierToAdd.bonusPenetration;
    }

    public void RemovePenetrationModifier(PenetrationModifier modifierToRemove)
    {
        penetrationModifiers.Remove(modifierToRemove);
        _bonusPenetration -= modifierToRemove.bonusPenetration;
    }

    public void AddSpreadModifierModifier(SpreadModifier modifierToAdd)
    {
        spreadModifiers.Add(modifierToAdd);
        _bonusSpread += (_bonusSpread * modifierToAdd.bonusSpread);
    }

    public void RemoveSpreadModifier(SpreadModifier modifierToRemove)
    {
        spreadModifiers.Remove(modifierToRemove);
        _bonusSpread /= (1 + modifierToRemove.bonusSpread);
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        fireRateModifiers.Add(modifierToAdd);
        _bonusFireRate += (_bonusFireRate * modifierToAdd.bonusAttackSpeed);

    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {
        fireRateModifiers.Remove(modifierToRemove);
        _bonusFireRate /= (1 + modifierToRemove.bonusAttackSpeed);

    }

    public void AddReloadSpeedModifier(ReloadSpeedModifier modifierToAdd)
    {
        reloadSpeedModifiers.Add(modifierToAdd);
        _bonusReloadSpeed += (_bonusReloadSpeed * modifierToAdd.bonusReloadSpeed);
    }

    public void RemoveReloadSpeedModifier(ReloadSpeedModifier modifierToRemove)
    {
        reloadSpeedModifiers.Remove(modifierToRemove);
        _bonusReloadSpeed /= (1 + modifierToRemove.bonusReloadSpeed);
    }

    public void AddAmmoLoadModifier(AmmoLoadModifier modifierToAdd)
    {
        ammoLoadModifiers.Add(modifierToAdd);
        _bonusAmmoLoad += (_bonusAmmoLoad * modifierToAdd.bonusAmmoLoad);

        // Give player's weapon this bonus ammo load (this is because bulletsLoaded is only inside of the SO)
        // Refill the player's weapon before applying new ammo load
        RefillAmmoCompletely();
        
        _bulletsLoaded = Mathf.RoundToInt(_bulletsLoaded * _bonusAmmoLoad);
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
    }

    public void RemoveAmmoLoadModifier(AmmoLoadModifier modifierToRemove)
    {
        ammoLoadModifiers.Remove(modifierToRemove);
        _bonusAmmoLoad /= (1 + modifierToRemove.bonusAmmoLoad);
    }

    #endregion
}
