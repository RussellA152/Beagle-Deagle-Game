using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Gun : MonoBehaviour, IGunDataUpdatable
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    private IPlayerStatModifier playerStatModifierScript;

    private PlayerInput playerInput;

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
        playerEvents.givePlayerStatModifierScriptEvent += UpdatePlayerStatsModifierScript;
        playerEvents.givePlayerInputComponentEvent += SetPlayerInput;
    }

    private void OnEnable()
    {
        UpdateScriptableObject(weaponData);
    }
    private void Start()
    {
        playerInput.actions["Fire"].performed += OnFire;
        playerInput.actions["Reload"].performed += OnReload;
    }

    private void OnDestroy()
    {
        playerEvents.givePlayerStatModifierScriptEvent -= UpdatePlayerStatsModifierScript;
        playerEvents.givePlayerInputComponentEvent -= SetPlayerInput;
    }

    private void Update()
    {
        lastTimeShot += Time.deltaTime;

        if (weaponData.bulletsLoaded <= 0f)
        {
            Reload();
            return;
        }

        // if player is holding down "fire" button, then attempt to shoot
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

    public void OnReload(CallbackContext context)
    {
        Reload();
    }

    public void ShootGun()
    {
        weaponData.bulletSpawnPoint = bulletSpawnPoint;

        GameObject bullet;

        // fetch a bullet from object pooler
        bullet = ObjectPooler.instance.GetPooledObject(bulletPool.PoolKey);

        if (bullet != null)
        {
            // Player has shot their gun, so reset lastTimeShot to 0 seconds
            lastTimeShot = 0f;

            Bullet projectile = bullet.GetComponent<Bullet>();

            projectile.UpdateWeaponData(weaponData);

            projectile.UpdatePlayerStatsModifierScript(playerStatModifierScript);

            // pass that bullet into the weaponData's fire function
            weaponData.Fire(projectile, weaponData.bulletSpread * playerStatModifierScript.GetWeaponSpreadModifier());

            
        }
        else
        {
            Debug.Log("Could not retrieve a bullet from object pool!");
        }
    }

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

    public void SetPlayerInput(PlayerInput inputComponent)
    {
        playerInput = inputComponent;
    }

    public float ReturnLastTimeShot()
    {
        return lastTimeShot;
    }
}
