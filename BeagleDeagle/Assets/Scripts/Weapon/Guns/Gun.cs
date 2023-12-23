using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour, IGunDataUpdatable, IHasCooldown, IHasInput
{
    // Where does this bullet get shot from? (i.e the barrel)
    [SerializeField] private Transform bulletSpawnPoint;
    // Where does the muzzle flash appear at?
    [SerializeField] private SpriteRenderer muzzleFlash;
    
    // The sprite is a child gameObject of the weapon
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Required Components")]
    [SerializeField] private PlayerEvents playerEvents;
    [SerializeField] private SoundEvents soundEvents;
    // Player data determines which weapon to start with
    [SerializeField] private PlayerData playerData;
    private GunData _weaponData;

    private CooldownSystem _cooldownSystem;

    private PlayerInput _playerInput;
    

    private InputAction _shootInputAction;
    private InputAction _reloadInputAction;

    private float _shootInput; // Input for shooting
    private bool _canShoot;
    
    public bool ActuallyShooting { get; private set; } // Is the player shooting (i.e, not idle or reloading or just moving)
    
    private int _bulletsShot; // How much ammo has the player shot since the last reload or refill?
    
    private int _bulletsLoaded; // How much ammo is currently in the magazine?

    private bool _isReloading;
    private bool _canReload;
    
    // Is the player allowed to receive new weapons?
    private bool _canReceiveNewWeapon = true;
    [SerializeField] private List<GunData> _previousWeaponDatas = new List<GunData>();
    private Coroutine _weaponReceiveCoroutine;

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
        // PlayerInput component is located in parent gameObject (the Player)
        _playerInput = GetComponentInParent<PlayerInput>();
        
        // Fetch cooldown system component from the player gameObject (always the parent of their gun)
        _cooldownSystem = GetComponentInParent<CooldownSystem>();

        _weaponData = playerData.gunData;
        

        _shootInputAction = _playerInput.currentActionMap.FindAction("Fire");
        _reloadInputAction = _playerInput.currentActionMap.FindAction("Reload");

        
        _reloadInputAction.performed += OnReload;
        

    }

    private void Start()
    {
        Id = 11;
        CooldownDuration = _weaponData.totalReloadTime * _bonusReloadSpeed;
        
        UpdateScriptableObject(_weaponData);

        _bulletsLoaded = Mathf.RoundToInt(_bulletsLoaded * _bonusAmmoLoad);
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
        
        playerEvents.InvokeReloadCooldown(Id);

        _lastTimeShot = 0f;
        _timeElapsedSinceShot = 0f;

        _bulletsShot = 0;
        
        _bulletPoolKey = _weaponData.bulletPrefab.GetComponent<IPoolable>().PoolKey;
        
        muzzleFlash.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        _cooldownSystem.OnCooldownEnded += OnReloadFinish;

    }

    private void OnDisable()
    {
        _previousWeaponDatas.Clear();
        
        _cooldownSystem.OnCooldownEnded -= OnReloadFinish;
        _shootInputAction.Disable();
        _reloadInputAction.Disable();
    }

    private void Update()
    {
        // Always check if player is trying to shoot
        _shootInput = _shootInputAction.ReadValue<float>();
        
        //_lastTimeShot += Time.deltaTime;
        _timeElapsedSinceShot += Time.deltaTime;

        // If the player has no ammo loaded into their weapon, begin reloading
        // If the gun is not already reloading, begin a coroutine for the reload
        if (_bulletsLoaded <= 0f && !_cooldownSystem.IsOnCooldown(Id) && _reloadInputAction.enabled)
        {
            PerformReload();
            return;
        }

        // If player is holding down "fire" button, then attempt to shoot
        // Also check that the gun's fire rate is ready to shoot
        if (_shootInput > 0 &&  _canShoot && CheckAmmo() && CheckIfCanFire())
        {
            Attack();
            ActuallyShooting = true;
        }
        else
        {
            ActuallyShooting = false;
        }
    }
    
    private void UpdateWeaponSpriteScale()
    {
        spriteRenderer.transform.localScale = _weaponData.gunEffectsData.spriteScale;
        spriteRenderer.transform.localPosition = _weaponData.gunEffectsData.spritePosition;
    }
    
    // Update the GunData scriptable object to a new one (If the player is allowed to. It can be disabled by AWP sniper or by dying)
    // This can change many stats like damage, penetration, fireRate, appearance (sprite), and more.
    public void UpdateScriptableObject(GunData scriptableObject)
    {
        // Tell all listeners all previous weapons that this player has received and will receive
        if (!_previousWeaponDatas.Contains(scriptableObject))
        {
            _previousWeaponDatas.Add(scriptableObject);
            
            playerEvents.InvokeGiveAllUpdatedWeaponsEvent(_previousWeaponDatas);

        }

        // If the player is already trying to receive another weapon, cancel that previous coroutine
        if (_weaponReceiveCoroutine != null)
            StopCoroutine(_weaponReceiveCoroutine);
        
        _weaponReceiveCoroutine = StartCoroutine(WaitForWeaponUpdate(scriptableObject));
    }

    ///-///////////////////////////////////////////////////////////
    /// Wait until the player is allowed to receive a new weapon update.
    /// 
    private IEnumerator WaitForWeaponUpdate(GunData scriptableObject)
    {
        while (!_canReceiveNewWeapon)
        {
            yield return null;
        }
    
        _weaponData = scriptableObject;
    
        // Refill all ammo 
        RefillAmmoCompletely();
    
        _bulletsLoaded = Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad);
    
        // Change bulletSpawnPoint's position
        bulletSpawnPoint.localPosition = _weaponData.bulletSpawnLocation;
        
        // Change weapon and muzzle flashes (and their positions as well)
        spriteRenderer.sprite = _weaponData.gunEffectsData.weaponSprite;
        muzzleFlash.sprite = _weaponData.gunEffectsData.muzzleFlashSprite;
        muzzleFlash.transform.localPosition = _weaponData.gunEffectsData.muzzleFlashPosition;
        
        // Stop reloading if player switched to a new gun (ammo will refill anyways)
        _cooldownSystem.EndCooldown(Id);
        
        CooldownDuration = _weaponData.totalReloadTime * _bonusReloadSpeed;
        
        // After swapping to new weapon, show the ammo on the HUD
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
        
        playerEvents.InvokeNewWeaponEvent(_weaponData);
        
        playerEvents.InvokeUpdateMaxAmmoLoadedText(Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad));
        
        UpdateWeaponSpriteScale();

    }

    ///-///////////////////////////////////////////////////////////
    /// Allows or doesn't allow player to swap weapons.
    /// 
    public void AllowWeaponReceive(bool boolean)
    {
        _canReceiveNewWeapon = boolean;
    }

    ///-///////////////////////////////////////////////////////////
    /// Return the current gun data of this weapon
    /// 
    public GunData GetCurrentData()
    {
        return _weaponData;
    }
    
    #region Shooting
    
    public bool CheckIfCanFire()
    {
        return Time.time - _lastTimeShot > 1f / _weaponData.fireRate * _bonusFireRate;
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Fetch a bullet from object pooler, then shoot it out
    /// 
    public void Attack()
    {
        // Fetch a bullet from object pooler
        GameObject newBullet = ObjectPooler.Instance.GetPooledObject(_bulletPoolKey);

        if (newBullet != null)
        {
            // Tell BulletType to update the bullet with the data it needs
            // For example, give fire damage to the bullet, or give life steal values to the bullet
            // Pass in the bullet gameObject and the player gameObject(to retrieve modifiers)
            IBulletUpdatable projectile = newBullet.GetComponent<IBulletUpdatable>();

            // Update bullet's status effects with data, only if this weapon's bullets has status effects
            if (_weaponData.statusEffects != null)
            {
                foreach (IStatusEffect statusEffect in newBullet.GetComponents<IStatusEffect>())
                {
                    statusEffect.UpdateWeaponType(_weaponData.statusEffects);
                }
            }

            projectile.UpdateScriptableObject(_weaponData.bulletData);
            
            // Pass in the damage and penetration values of this gun, to the bullet being shot
            // Also account for any modifications to the gun damage and penetration (e.g, an item purchased by trader that increases player gun damage)
            projectile.UpdateDamageAndPenetrationValues(_weaponData.GetDamage() * _bonusDamage, _weaponData.penetrationCount + _bonusPenetration);
            

            // Set the position to be at the barrel of the gun
            newBullet.transform.position = bulletSpawnPoint.position;

            // Apply the spread to the bullet's rotation
            newBullet.transform.rotation = CalculateWeaponSpread();

            newBullet.gameObject.SetActive(true);
            
            projectile.ActivateBullet();

            _bulletsShot++;

            _bulletsLoaded--;
            
        }

        // Play a shoot sound effect
        PlayShootSound();
        // Play a muzzle flash effect
        StartCoroutine(PlayMuzzleFlash());

        // Player has shot gun, so reset timeElapsedSinceShot to 0 seconds
        _timeElapsedSinceShot = 0f;
        _lastTimeShot = Time.time;

        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
    }

    ///-///////////////////////////////////////////////////////////
    /// Turn on muzzle flash, then disable after a very short amount of time
    /// (depends on current weapon used).
    /// 
    private IEnumerator PlayMuzzleFlash()
    {
        muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(_weaponData.gunEffectsData.muzzleFlashDuration);
        muzzleFlash.gameObject.SetActive(false);
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Add a random offset to the bullet's Y position
    /// to simulate random spread.
    public virtual Quaternion CalculateWeaponSpread()
    {
        // Calculate the spread angle
        float spreadAngle = Random.Range(-_weaponData.bulletSpread * _bonusSpread, _weaponData.bulletSpread * _bonusSpread);

        return Quaternion.Euler(0f, 0f, spreadAngle) * bulletSpawnPoint.rotation;
    }

    public void AllowShoot(bool boolean)
    {
        if (boolean && !_cooldownSystem.IsOnCooldown(Id))
        {
            _canShoot = true;
        }
        else
        {
            _canShoot = false;
        }
    }
    
    public float ReturnLastTimeShot()
    {
        return _timeElapsedSinceShot;
    }

    #endregion

    #region Reloading
    
    private void PerformReload()
    {
        if (!_cooldownSystem.IsOnCooldown(Id) && _canReload)
        {
            _shootInput = 0f;
            _cooldownSystem.PutOnCooldown(this);
            
            // Play "reloadStart" sound effect
            soundEvents.InvokeGunSoundPlay(_weaponData.gunEffectsData.reloadStartClip, _weaponData.gunEffectsData.reloadSoundVolume);
            // Start playing the reload finished sound effect
            StartCoroutine(PlayReloadFinishedSound());
            
            ActuallyShooting = false;
            AllowShoot(false);
        }
    }
    
    // Call reload function when the player presses the reload key
    public void OnReload(CallbackContext context)
    {
        // Only allow manual reloading if the player has some ammo, but not empty
        if(_bulletsLoaded > 0f && _bulletsLoaded < _weaponData.magazineSize * _bonusAmmoLoad)
            PerformReload();
    }

    private void OnReloadFinish(int id)
    {
        if (id != Id) return;

        RefillAmmoCompletely();
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);

        AllowShoot(true);

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
        _bulletsLoaded = Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad);
        _bulletsShot = 0;
    }
    
    public void AllowReload(bool boolean)
    {
        _canReload = boolean;
    }
    #endregion
    
    #region Sounds

    private void PlayShootSound()
    {
        int randomNumber = Random.Range(0, _weaponData.gunEffectsData.fireClips.Length);
        
        soundEvents.InvokeGunSoundPlay(_weaponData.gunEffectsData.fireClips[randomNumber], _weaponData.gunEffectsData.fireSoundVolume);
    }

    private IEnumerator PlayReloadFinishedSound()
    {
        float halfDuration = CooldownDuration / 2f;
        float eightyPercentDuration = CooldownDuration * 0.8f;

        // Wait for 50% of the cooldown duration
        yield return new WaitForSeconds(halfDuration);
    
        // Play "reloadFinished" sound effect at 50% completion
        soundEvents.InvokeGunSoundPlay(_weaponData.gunEffectsData.reloadFinishedClip, _weaponData.gunEffectsData.reloadSoundVolume);

        // If the gun is not empty upon reloading, then don't play a reload slide sound
        if (_bulletsLoaded != 0)
            yield break;

        // Calculate the remaining time to reach 80% completion
        float remainingTime = eightyPercentDuration - halfDuration;
    
        // Wait for the remaining time
        yield return new WaitForSeconds(remainingTime);

        // Play "reloadSlide" sound effect at 80% completion
        soundEvents.InvokeGunSoundPlay(_weaponData.gunEffectsData.reloadSlideClip, _weaponData.gunEffectsData.reloadSoundVolume);
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

    public void AddSpreadModifier(SpreadModifier modifierToAdd)
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
        
        CooldownDuration = _weaponData.totalReloadTime * _bonusReloadSpeed;
    }

    public void RemoveReloadSpeedModifier(ReloadSpeedModifier modifierToRemove)
    {
        reloadSpeedModifiers.Remove(modifierToRemove);
        _bonusReloadSpeed /= (1 + modifierToRemove.bonusReloadSpeed);
        
        CooldownDuration = _weaponData.totalReloadTime * _bonusReloadSpeed;
        
    }

    public void AddAmmoLoadModifier(AmmoLoadModifier modifierToAdd)
    {
        ammoLoadModifiers.Add(modifierToAdd);
        _bonusAmmoLoad += (_bonusAmmoLoad * modifierToAdd.bonusAmmoLoad);
        
        // Give player's weapon this bonus ammo load (this is because bulletsLoaded is only inside of the SO)
        // Refill the player's weapon before applying new ammo load
        RefillAmmoCompletely();

        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
        playerEvents.InvokeUpdateMaxAmmoLoadedText(Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad));
        
    }

    public void RemoveAmmoLoadModifier(AmmoLoadModifier modifierToRemove)
    {
        ammoLoadModifiers.Remove(modifierToRemove);
        _bonusAmmoLoad /= (1 + modifierToRemove.bonusAmmoLoad);
        
        _bulletsLoaded = Mathf.RoundToInt(_bulletsLoaded * _bonusAmmoLoad);
        
        playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
        playerEvents.InvokeUpdateMaxAmmoLoadedText(Mathf.RoundToInt(_weaponData.magazineSize * _bonusAmmoLoad));
    }

    #endregion

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
    
    public void AllowInput(bool boolean)
    {
        if (boolean)
        {
            _shootInputAction.Enable();
            _reloadInputAction.Enable();
        }
        else
        {
            _shootInputAction.Disable();
            _reloadInputAction.Disable();
        }
    }
}
