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

    private float shootInput; // Input for shooting

    private float lastTimeShot;

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

    private float bonusDamage = 1f;
    private int bonusPenetration = 0;
    private float bonusSpread = 1f;
    private float bonusFireRate = 1f;
    private float bonusReloadSpeed = 1f;
    private float bonusAmmoLoad = 1f;

    private void Start()
    {
        playerEvents.InvokeUpdateAmmoLoadedText(weaponData.bulletsLoaded);
    }

    private void OnEnable()
    {
        UpdateScriptableObject(weaponData);
        
    }

    private void Update()
    {
        lastTimeShot += Time.deltaTime;

        // If the player has no ammo loaded into their weapon, begin reloading
        // If the gun is not already reloading, begin a coroutine for the reload
        if (weaponData.bulletsLoaded <= 0f && !weaponData.isReloading)
        {
            StartCoroutine(Reload());
            return;
        }

        // If player is holding down "fire" button, then attempt to shoot
        // Also check that the gun's fire rate is ready to shoot
        if (shootInput > 0 && weaponData.CheckAmmo() && weaponData.CheckIfCanFire(bonusFireRate))
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
        shootInput = context.ReadValue<float>();

    }

    public IEnumerator Reload()
    {
        // Wait until reload is finished
        yield return StartCoroutine(weaponData.WaitReload(bonusReloadSpeed));

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
        lastTimeShot = 0f;

        int ammoLoaded = weaponData.Fire(ObjectPooler.instance, bonusDamage, bonusSpread, bonusPenetration);

        playerEvents.InvokeUpdateAmmoLoadedText(ammoLoaded);
    }

    // Update the GunData scriptable object to a new one.
    // This can change many stats like damage, penetration, fireRate, appearance (sprite), and more.
    public void UpdateScriptableObject(GunData scriptableObject)
    {

        weaponData.bulletSpawnPoint = bulletSpawnPoint;

        weaponData = scriptableObject;

        weaponData.bulletsLoaded = Mathf.RoundToInt(weaponData.bulletsLoaded * bonusAmmoLoad);

        spriteRenderer.sprite = weaponData.sprite;
    }

    public float ReturnLastTimeShot()
    {
        return lastTimeShot;
    }

    #region StatModifiers
    public void AddDamageModifier(DamageModifier modifierToAdd)
    {
        damageModifiers.Add(modifierToAdd);
        bonusDamage += (bonusDamage * modifierToAdd.bonusDamage);

    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        damageModifiers.Remove(modifierToRemove);
        bonusDamage /= (1 + modifierToRemove.bonusDamage);
    }


    public void AddPenetrationModifier(PenetrationModifier modifierToAdd)
    {
        penetrationModifiers.Add(modifierToAdd);
        bonusPenetration += modifierToAdd.bonusPenetration;
    }

    public void RemovePenetrationModifier(PenetrationModifier modifierToRemove)
    {
        penetrationModifiers.Remove(modifierToRemove);
        bonusPenetration -= modifierToRemove.bonusPenetration;
    }

    public void AddSpreadModifierModifier(SpreadModifier modifierToAdd)
    {
        spreadModifiers.Add(modifierToAdd);
        bonusSpread += (bonusSpread * modifierToAdd.bonusSpread);
    }

    public void RemoveSpreadModifier(SpreadModifier modifierToRemove)
    {
        spreadModifiers.Remove(modifierToRemove);
        bonusSpread /= (1 + modifierToRemove.bonusSpread);
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        fireRateModifiers.Add(modifierToAdd);
        bonusFireRate += (bonusFireRate * modifierToAdd.bonusAttackSpeed);

    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {
        fireRateModifiers.Remove(modifierToRemove);
        bonusFireRate /= (1 + modifierToRemove.bonusAttackSpeed);

    }

    public void AddReloadSpeedModifier(ReloadSpeedModifier modifierToAdd)
    {
        reloadSpeedModifiers.Add(modifierToAdd);
        bonusReloadSpeed += (bonusReloadSpeed * modifierToAdd.bonusReloadSpeed);
    }

    public void RemoveReloadSpeedModifier(ReloadSpeedModifier modifierToRemove)
    {
        reloadSpeedModifiers.Remove(modifierToRemove);
        bonusReloadSpeed /= (1 + modifierToRemove.bonusReloadSpeed);
    }

    public void AddAmmoLoadModifier(AmmoLoadModifier modifierToAdd)
    {
        ammoLoadModifiers.Add(modifierToAdd);
        bonusAmmoLoad += (bonusAmmoLoad * modifierToAdd.bonusAmmoLoad);

        // Give player's weapon this bonus ammo load (this is because bulletsLoaded is only inside of the SO)
        // Refill the player's weapon before applying new ammo load
        weaponData.RefillAmmo();
        weaponData.bulletsLoaded = Mathf.RoundToInt(weaponData.bulletsLoaded * bonusAmmoLoad);
        playerEvents.InvokeUpdateAmmoLoadedText(weaponData.bulletsLoaded);
    }

    public void RemoveAmmoLoadModifier(AmmoLoadModifier modifierToRemove)
    {
        ammoLoadModifiers.Remove(modifierToRemove);
        bonusAmmoLoad /= (1 + modifierToRemove.bonusAmmoLoad);
    }

    #endregion
}
