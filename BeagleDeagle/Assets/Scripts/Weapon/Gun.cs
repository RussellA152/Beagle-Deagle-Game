using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Gun : MonoBehaviour, IGunDataUpdatable, IDamager
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    public GunData weaponData;

    [SerializeField] 
    private Transform bulletSpawnPoint; // Where does this bullet get shot from? (i.e the barrel)

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float _shootInput; // Input for shooting

    private float _lastTimeShot;

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

    private void Start()
    {
        playerEvents.InvokeUpdateAmmoLoadedText(Mathf.RoundToInt(weaponData.bulletsLoaded * _bonusAmmoLoad));
    }

    private void OnEnable()
    {
        playerEvents.playerObtainedNewWeaponEvent += UpdateScriptableObject;

        playerEvents.InvokeNewWeaponEvent(weaponData);
        //UpdateScriptableObject(weaponData);
        
    }

    private void Update()
    {
        _lastTimeShot += Time.deltaTime;

        // If the player has no ammo loaded into their weapon, begin reloading
        // If the gun is not already reloading, begin a coroutine for the reload
        if (weaponData.bulletsLoaded <= 0f && !weaponData.isReloading)
        {
            StartCoroutine(Reload());
            return;
        }

        // If player is holding down "fire" button, then attempt to shoot
        // Also check that the gun's fire rate is ready to shoot
        if (_shootInput > 0 && weaponData.CheckAmmo() && weaponData.CheckIfCanFire(_bonusFireRate))
        {
            Attack();
        }
        else
        {
            weaponData.actuallyShooting = false;
        }
    }
    public void OnFire(CallbackContext context)
    {
        _shootInput = context.ReadValue<float>();

    }

    public IEnumerator Reload()
    {
        // Wait until reload is finished
        yield return StartCoroutine(weaponData.WaitReload(_bonusReloadSpeed));

        // Then call event that ammo has changed
        playerEvents.InvokeUpdateAmmoLoadedText(weaponData.bulletsLoaded);
    }

    // Call reload function when the player presses the reload key
    public void OnReload(CallbackContext context)
    {
        StartCoroutine(Reload());
    }

    // Fetch a bullet from object pooler, then pass it into the GunData's Fire() method so it can shoot it
    public void Attack()
    {
        // The bullet will spawn at the barrel of the gun
        weaponData.bulletSpawnPoint = bulletSpawnPoint;

        // Player has shot their gun, so reset lastTimeShot to 0 seconds
        _lastTimeShot = 0f;

        int ammoLoaded = weaponData.Fire(ObjectPooler.instance, _bonusDamage, _bonusSpread, _bonusPenetration);

        playerEvents.InvokeUpdateAmmoLoadedText(ammoLoaded);
    }

    // Update the GunData scriptable object to a new one.
    // This can change many stats like damage, penetration, fireRate, appearance (sprite), and more.
    public void UpdateScriptableObject(GunData scriptableObject)
    {
        // Refill the ammo of the old weapon data before switching to new data
        weaponData.RefillAmmo();

        weaponData.bulletSpawnPoint = bulletSpawnPoint;

        weaponData = scriptableObject;

        weaponData.bulletsLoaded = Mathf.RoundToInt(weaponData.bulletsLoaded * _bonusAmmoLoad);

        spriteRenderer.sprite = weaponData.sprite;

        // After swapping to new weapon, show the ammo on the HUD
        playerEvents.InvokeUpdateAmmoLoadedText(weaponData.bulletsLoaded);
    }
    public GunData GetCurrentData()
    {
        return weaponData;
    }

    public float ReturnLastTimeShot()
    {
        return _lastTimeShot;
    }

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
        weaponData.RefillAmmo();
        weaponData.bulletsLoaded = Mathf.RoundToInt(weaponData.bulletsLoaded * _bonusAmmoLoad);
        playerEvents.InvokeUpdateAmmoLoadedText(weaponData.bulletsLoaded);
    }

    public void RemoveAmmoLoadModifier(AmmoLoadModifier modifierToRemove)
    {
        ammoLoadModifiers.Remove(modifierToRemove);
        _bonusAmmoLoad /= (1 + modifierToRemove.bonusAmmoLoad);
    }

    #endregion
}
