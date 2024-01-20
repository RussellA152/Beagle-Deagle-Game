using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Random = UnityEngine.Random;

public class GunShooting : MonoBehaviour, IRegisterModifierMethods, IHasInput, IGunDataUpdatable
{
    [Header("Required Components")]
    [SerializeField] private PlayerEvents playerEvents;
    
    private AudioClipPlayer _audioClipPlayer;
    private GunData _weaponData;
    private CameraShaker _cameraShaker;
    private ModifierManager _modifierManager;
    private MiscellaneousModifierList _miscellaneousModifierList;
    private GunReload _gunReload;
    
    private PlayerInput _playerInput;
    
    // Where does this bullet get shot from? (i.e the barrel)
    [SerializeField] private Transform bulletSpawnPoint;
    // Where does the muzzle flash appear at?
    [SerializeField] private SpriteRenderer muzzleFlash;
    
    private InputAction _shootInputAction;
    
    private float _shootInput; // Input for shooting
    private bool _canShoot;
    
    public bool ActuallyShooting { get; private set; } // Is the player shooting (i.e, not idle or reloading or just moving)
    
    
    private float _lastTimeShot;
    private float _timeElapsedSinceShot;
    
    private int _bulletPoolKey;
    
    [Header("Modifiers")]
    [SerializeField, NonReorderable]
    private List<DamageModifier> damageModifiers = new List<DamageModifier>(); // A bonus percentage applied to the gun's damage
    [SerializeField, NonReorderable]
    private List<CriticalChanceModifier> criticalChanceModifiers = new List<CriticalChanceModifier>();
    [SerializeField, NonReorderable]
    private List<PenetrationModifier> penetrationModifiers = new List<PenetrationModifier>();
    [SerializeField, NonReorderable]
    private List<SpreadModifier> spreadModifiers = new List<SpreadModifier>();
    [SerializeField, NonReorderable]
    private List<AttackSpeedModifier> fireRateModifiers = new List<AttackSpeedModifier>();
    
    private float _bonusDamage = 1f;
    private int _bonusPenetration = 0;
    private float _bonusSpread = 1f;
    private float _bonusFireRate = 1f;
    private float _bonusCriticalChance = 0f;

    private void Awake()
    {
        // PlayerInput component is located in parent gameObject (the Player)
        _playerInput = GetComponentInParent<PlayerInput>();
        
        _audioClipPlayer = GetComponentInParent<AudioClipPlayer>();
        _cameraShaker = GetComponent<CameraShaker>();

        _modifierManager = GetComponentInParent<ModifierManager>();
        
        _miscellaneousModifierList = GetComponentInParent<MiscellaneousModifierList>();

        _gunReload = GetComponent<GunReload>();
        
        _shootInputAction = _playerInput.currentActionMap.FindAction("Fire");
        
        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();
    }

    private void Start()
    {
        _lastTimeShot = 0f;
        _timeElapsedSinceShot = 0f;
        
        _bulletPoolKey = _weaponData.bulletPrefab.GetComponent<IPoolable>().PoolKey;

        muzzleFlash.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _shootInputAction.Disable();
    }

    private void Update()
    {
        // Always check if player is trying to shoot
        _shootInput = _shootInputAction.ReadValue<float>();
        
        _timeElapsedSinceShot += Time.deltaTime;

        if (_gunReload.IsReloading)
        {
            _shootInput = 0f;
            AllowShoot(false);
            ActuallyShooting = false;
        }
            
        
        // If player is holding down "fire" button, then attempt to shoot
        // Also check that the gun's fire rate is ready to shoot
        if (_shootInput > 0 &&  _canShoot && _gunReload.CheckAmmo() && CheckIfCanFire())
        {
            Attack();
            ActuallyShooting = true;
        }
        else
        {
            ActuallyShooting = false;
        }
    }
    
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
                    statusEffect.UpdateStatusDataTypes(_weaponData.statusEffects);
                }
            }

            // Give bullet any extra modifiers (ex. giving an explosive bullet "bonusExplosiveRadius")
            foreach (IHasMiscellaneousModifier hasMiscellaneousModifier in newBullet.GetComponents<IHasMiscellaneousModifier>())
            {
                hasMiscellaneousModifier.GiveMiscellaneousModifierList(_miscellaneousModifierList);
            }
            
            // Increase dot damage, if this gun deals more damage.
            foreach (IApplyDamageOverTime applyDamageOverTime in newBullet.GetComponents<IApplyDamageOverTime>())
            {
                applyDamageOverTime.GiveBonusDamage(_bonusDamage);
            }

            projectile.UpdateScriptableObject(_weaponData.bulletData);
            
            // Pass in the damage and penetration values of this gun, to the bullet being shot
            // Also account for any modifications to the gun damage and penetration (e.g, an item purchased by trader that increases player gun damage)
            projectile.UpdateDamage(CalculateDamage(projectile));
            projectile.UpdatePenetration(_weaponData.penetrationCount + _bonusPenetration);
            

            // Set the position to be at the barrel of the gun
            newBullet.transform.position = bulletSpawnPoint.position;

            // Apply the spread to the bullet's rotation
            newBullet.transform.rotation = CalculateWeaponSpread();

            newBullet.gameObject.SetActive(true);
            
            projectile.ActivateBullet();

            // _bulletsShot++;
            //
            // _bulletsLoaded--;
            
            _gunReload.UpdateAmmo();
            
        }

        // Play a shoot sound effect
        PlayShootSound();
        
        // Tell camera shaker to shake the player's camera
        _cameraShaker.ShakePlayerCamera(_weaponData.gunEffectsData.screenShakeData);
        
        // Play a muzzle flash effect
        StartCoroutine(PlayMuzzleFlash());

        // Player has shot gun, so reset timeElapsedSinceShot to 0 seconds
        _timeElapsedSinceShot = 0f;
        _lastTimeShot = Time.time;

        // playerEvents.InvokeUpdateAmmoLoadedText(_bulletsLoaded);
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

    ///-///////////////////////////////////////////////////////////
    /// Return damage of the gun after calculating any critical hit chance and bonus damage.
    /// 
    private float CalculateDamage(IBulletUpdatable projectile)
    {
        float damageToDeal = _weaponData.GetBaseDamage() * _bonusDamage;

        if (Random.value < _weaponData.criticalChance + _bonusCriticalChance)
        {
            projectile.SetIsCrit(true);
            return damageToDeal * _weaponData.criticalHitMultiplier;
        }
        
        projectile.SetIsCrit(false);
        
        return damageToDeal;
    }

    public void AllowShoot(bool boolean)
    {
        if (boolean && !_gunReload.IsReloading)
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
    
    private void PlayShootSound()
    {
        int randomNumber = Random.Range(0, _weaponData.gunEffectsData.fireClips.Length);
        
        _audioClipPlayer.PlayGeneralAudioClip(_weaponData.gunEffectsData.fireClips[randomNumber], _weaponData.gunEffectsData.fireSoundVolume);
    }
    
    public void AddDamageModifier(DamageModifier modifierToAdd)
    {
        if (damageModifiers.Contains(modifierToAdd)) return;
        
        damageModifiers.Add(modifierToAdd);
        _bonusDamage += _bonusDamage * modifierToAdd.bonusDamage;
    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        if (!damageModifiers.Contains(modifierToRemove)) return;
        
        damageModifiers.Remove(modifierToRemove);
        _bonusDamage /= (1 + modifierToRemove.bonusDamage);
    }


    public void AddPenetrationModifier(PenetrationModifier modifierToAdd)
    {
        if (penetrationModifiers.Contains(modifierToAdd)) return;
        
        penetrationModifiers.Add(modifierToAdd);
        _bonusPenetration += modifierToAdd.bonusPenetration;
    }

    public void RemovePenetrationModifier(PenetrationModifier modifierToRemove)
    {
        if (!penetrationModifiers.Contains(modifierToRemove)) return;
        
        penetrationModifiers.Remove(modifierToRemove);
        _bonusPenetration -= modifierToRemove.bonusPenetration;
    }

    public void AddSpreadModifier(SpreadModifier modifierToAdd)
    {
        if (spreadModifiers.Contains(modifierToAdd)) return;
        
        spreadModifiers.Add(modifierToAdd);
        _bonusSpread += _bonusSpread * modifierToAdd.bonusSpread;
    }

    public void RemoveSpreadModifier(SpreadModifier modifierToRemove)
    {
        if (!spreadModifiers.Contains(modifierToRemove)) return;
        
        spreadModifiers.Remove(modifierToRemove);
        _bonusSpread /= (1 + modifierToRemove.bonusSpread);
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        if (fireRateModifiers.Contains(modifierToAdd)) return;
        
        fireRateModifiers.Add(modifierToAdd);
        _bonusFireRate += _bonusFireRate * modifierToAdd.bonusAttackSpeed;
    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {
        if (!fireRateModifiers.Contains(modifierToRemove)) return;
        
        fireRateModifiers.Remove(modifierToRemove);
        _bonusFireRate /= (1 + modifierToRemove.bonusAttackSpeed);

    }
    public void AddCriticalHitChanceModifier(CriticalChanceModifier modifierToAdd)
    {
        if (criticalChanceModifiers.Contains(modifierToAdd)) return;
        
        criticalChanceModifiers.Add(modifierToAdd);
        _bonusCriticalChance += (_bonusFireRate * modifierToAdd.bonusCriticalChance);
    }

    public void RemoveCriticalHitChanceModifier(CriticalChanceModifier modifierToRemove)
    {
        if (!criticalChanceModifiers.Contains(modifierToRemove)) return;
        
        criticalChanceModifiers.Remove(modifierToRemove);
        _bonusCriticalChance /= (1 + modifierToRemove.bonusCriticalChance);
    }
    
    public void AllowInput(bool boolean)
    {
        if (boolean)
        {
            _shootInputAction.Enable();
        }
        else
        {
           _shootInputAction.Disable();
        }
    }

    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<DamageModifier>(AddDamageModifier);
        _modifierManager.RegisterAddMethod<PenetrationModifier>(AddPenetrationModifier);
        _modifierManager.RegisterAddMethod<SpreadModifier>(AddSpreadModifier);
        _modifierManager.RegisterAddMethod<AttackSpeedModifier>(AddAttackSpeedModifier);
        _modifierManager.RegisterAddMethod<CriticalChanceModifier>(AddCriticalHitChanceModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<DamageModifier>(RemoveDamageModifier);
        _modifierManager.RegisterRemoveMethod<PenetrationModifier>(RemovePenetrationModifier);
        _modifierManager.RegisterRemoveMethod<SpreadModifier>(RemoveSpreadModifier);
        _modifierManager.RegisterRemoveMethod<AttackSpeedModifier>(RemoveAttackSpeedModifier);
        _modifierManager.RegisterRemoveMethod<CriticalChanceModifier>(RemoveCriticalHitChanceModifier);
    }

    public void UpdateScriptableObject(GunData scriptableObject)
    {
        _weaponData = scriptableObject;
        
        // Fetch new bullets from object pool
        _bulletPoolKey = _weaponData.bulletPrefab.GetComponent<IPoolable>().PoolKey;
        
        // Change bulletSpawnPoint's position
        bulletSpawnPoint.localPosition = _weaponData.bulletSpawnLocation;
        
        muzzleFlash.sprite = _weaponData.gunEffectsData.muzzleFlashSprite;
        muzzleFlash.transform.localPosition = _weaponData.gunEffectsData.muzzleFlashPosition;
    }

    public GunData GetCurrentData()
    {
        return _weaponData;
    }
}
