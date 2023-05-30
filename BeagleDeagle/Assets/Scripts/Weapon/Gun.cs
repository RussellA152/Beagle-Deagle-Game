using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Gun : MonoBehaviour, IGunDataUpdatable, IDamager
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    //private IPlayerStatModifier playerStatModifierScript;

    public GunData weaponData;

    [SerializeField] 
    private Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float shootInput; // input for shooting

    //private IPoolable bulletPool;

    private float lastTimeShot;

    [SerializeField, NonReorderable]
    private List<DamageModifier> damageModifiers = new List<DamageModifier>(); // a bonus percentage applied to the gun's damage
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

        //bulletPool = weaponData.bullet.GetComponent<IPoolable>();

        weaponData.bulletsLoaded = Mathf.RoundToInt(weaponData.bulletsLoaded * bonusAmmoLoad);

        spriteRenderer.sprite = weaponData.sprite;
    }

    //public void UpdatePlayerStatsModifierScript(IPlayerStatModifier modifierScript)
    //{
    //playerStatModifierScript = modifierScript;
    //}

    public float ReturnLastTimeShot()
    {
        return lastTimeShot;
    }

    #region StatModifiers
    public void AddDamageModifier(DamageModifier modifierToAdd)
    {
        if (!damageModifiers.Contains(modifierToAdd))
        {
            damageModifiers.Add(modifierToAdd);
            bonusDamage += modifierToAdd.bonusDamage;
        }
        else
        {
            damageModifiers.Add(modifierToAdd);
        }

    }

    public void RemoveDamageModifier(DamageModifier modifierToRemove)
    {
        if (!modifierToRemove.appliedOnTriggerEnter)
        {
            int count = damageModifiers.FindAll(num => num == modifierToRemove).Count;

            if (count > 1)
            {
                damageModifiers.Remove(modifierToRemove);
                Debug.Log("Keep slow effect!");
            }
            else if (count == 1)
            {
                damageModifiers.Remove(modifierToRemove);
                bonusDamage -= modifierToRemove.bonusDamage;

                Debug.Log("Remove slow effect permanently!");
            }
        }
        else
        {
            damageModifiers.Remove(modifierToRemove);
            bonusDamage -= modifierToRemove.bonusDamage;
        }
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
        bonusSpread += modifierToAdd.bonusSpread;
    }

    public void RemoveSpreadModifier(SpreadModifier modifierToRemove)
    {
        spreadModifiers.Remove(modifierToRemove);
        bonusSpread -= modifierToRemove.bonusSpread;
    }

    public void AddAttackSpeedModifier(AttackSpeedModifier modifierToAdd)
    {
        if (!fireRateModifiers.Contains(modifierToAdd))
        {
            fireRateModifiers.Add(modifierToAdd);
            bonusFireRate += modifierToAdd.bonusAttackSpeed;
        }
        else
        {
            fireRateModifiers.Add(modifierToAdd);
        }

    }

    public void RemoveAttackSpeedModifier(AttackSpeedModifier modifierToRemove)
    {

        if (!modifierToRemove.appliedOnTriggerEnter)
        {
            int count = fireRateModifiers.FindAll(num => num == modifierToRemove).Count;

            if (count > 1)
            {
                fireRateModifiers.Remove(modifierToRemove);
                Debug.Log("Keep slow effect!");
            }
            else if (count == 1)
            {
                fireRateModifiers.Remove(modifierToRemove);
                bonusFireRate -= modifierToRemove.bonusAttackSpeed;

                Debug.Log("Remove slow effect permanently!");
            }
        }
        else
        {
            fireRateModifiers.Remove(modifierToRemove);
            bonusFireRate -= modifierToRemove.bonusAttackSpeed;
        }

    }

    public void AddReloadSpeedModifier(ReloadSpeedModifier modifierToAdd)
    {
        reloadSpeedModifiers.Add(modifierToAdd);
        bonusReloadSpeed += modifierToAdd.bonusReloadSpeed;
    }

    public void RemoveReloadSpeedModifier(ReloadSpeedModifier modifierToRemove)
    {
        reloadSpeedModifiers.Remove(modifierToRemove);
        bonusReloadSpeed-= modifierToRemove.bonusReloadSpeed;
    }

    public void AddAmmoLoadModifier(AmmoLoadModifier modifierToAdd)
    {
        ammoLoadModifiers.Add(modifierToAdd);
        bonusAmmoLoad += modifierToAdd.bonusAmmoLoad;

        // Give player's weapon this bonus ammo load (this is because bulletsLoaded is only inside of the SO)
        // Refill the player's weapon before applying new ammo load
        weaponData.RefillAmmo();
        weaponData.bulletsLoaded = Mathf.RoundToInt(weaponData.bulletsLoaded * bonusAmmoLoad);
        playerEvents.InvokeUpdateAmmoLoadedText(weaponData.bulletsLoaded);
    }

    public void RemoveAmmoLoadModifier(AmmoLoadModifier modifierToRemove)
    {
        ammoLoadModifiers.Remove(modifierToRemove);
        bonusAmmoLoad -= modifierToRemove.bonusAmmoLoad;
    }

    #endregion
}
