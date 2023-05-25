using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Gun : MonoBehaviour, IGunDataUpdatable
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    private IPlayerStatModifier playerStatModifierScript;

    public GunData weaponData;

    [SerializeField] 
    private Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float shootInput; // input for shooting

    private IPoolable bulletPool;

    private float lastTimeShot;

    private void Awake()
    {
        // Retrieve a reference to the PlayerStatModifier script when it gets enabled
        playerEvents.givePlayerStatModifierScriptEvent += UpdatePlayerStatsModifierScript;
    }

    private void OnEnable()
    {
        UpdateScriptableObject(weaponData);
        
    }

    private void OnDestroy()
    {
        playerEvents.givePlayerStatModifierScriptEvent -= UpdatePlayerStatsModifierScript;
    }

    private void Update()
    {
        lastTimeShot += Time.deltaTime;

        // If the player has no ammo loaded into their weapon, begin reloading
        if (weaponData.bulletsLoaded <= 0f)
        {
            Reload();
            return;
        }

        // If player is holding down "fire" button, then attempt to shoot
        // Also check that the gun's fire rate is ready to shoot
        if (shootInput > 0 && weaponData.CheckAmmo() && weaponData.CheckIfCanFire(weaponData.fireRate * playerStatModifierScript.GetAttackSpeedModifier()))
        {
            ShootGun();
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

    public void Reload() {
        // If the gun is not already reloading, begin a coroutine for the reload
        if (!weaponData.isReloading)
            // The duration of the reload is the gunData's reloadTime * a reload speed modifier (this takes into account any items that affect the reload speed)
            StartCoroutine(weaponData.WaitReload(weaponData.totalReloadTime * playerStatModifierScript.GetWeaponReloadSpeedModifier()));
    }

    // Call reload function when the player presses the reload key
    public void OnReload(CallbackContext context)
    {
        Reload();
    }

    // Fetch a bullet from object pooler, then pass it into the GunData's Fire() method so it can shoot it
    public void ShootGun()
    {
        // The bullet will spawn at the barrel of the gun
        weaponData.bulletSpawnPoint = bulletSpawnPoint;

        GameObject bullet;

        // Fetch a bullet from object pooler
        bullet = ObjectPooler.instance.GetPooledObject(bulletPool.PoolKey);

        if (bullet != null)
        {
            // Player has shot their gun, so reset lastTimeShot to 0 seconds
            lastTimeShot = 0f;

            Bullet projectile = bullet.GetComponent<Bullet>();

            // Pass in the damage and penetration values of this gun, to the bullet being shot
            // Also account for any modifications to the gun damage and penetration (e.g, an item purchased by trader that increases player gun damage)
            projectile.UpdateWeaponValues(weaponData.damagePerHit * playerStatModifierScript.GetDamageModifier(), weaponData.penetrationCount * playerStatModifierScript.GetPenetrationCountModifier());

            // Giving the bullet its data (for the 'destroyTime' variable and 'trajectory' method)
            projectile.UpdateProjectileData(weaponData.bulletData);
       

            // Set the position to be at the barrel of the gun
            bullet.transform.position = bulletSpawnPoint.position;

            // Apply the spread to the bullet's rotation
            bullet.transform.rotation = weaponData.CalculateWeaponSpread(bulletSpawnPoint.rotation, weaponData.bulletSpread * playerStatModifierScript.GetWeaponSpreadModifier());

            // Pass that bullet into the weaponData's fire function
            weaponData.Fire(projectile);
            
        }
        else
        {
            Debug.Log("Could not retrieve a bullet from object pool!");
        }
    }

    // Update the GunData scriptable object to a new one.
    // This can change many stats like damage, penetration, fireRate, appearance (sprite), and more.
    public void UpdateScriptableObject(GunData scriptableObject)
    {
        weaponData.bulletSpawnPoint = bulletSpawnPoint;

        weaponData = scriptableObject;

        bulletPool = weaponData.bullet.GetComponent<IPoolable>();

        spriteRenderer.sprite = weaponData.sprite;
    }

    public void UpdatePlayerStatsModifierScript(IPlayerStatModifier modifierScript)
    {
       playerStatModifierScript = modifierScript;
    }

    public float ReturnLastTimeShot()
    {
        return lastTimeShot;
    }
}
