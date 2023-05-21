using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Gun : MonoBehaviour, IGunDataUpdatable
{
    private PlayerInput playerInput;

    public GunData weaponData;

    [SerializeField] private Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float shootInput; // input for shooting

    private IPoolable bulletPool;


    private void Start()
    {
        playerInput = PlayerManager.instance.GetPlayerInput();

        playerInput.actions["Fire"].performed += OnFire;
        playerInput.actions["Reload"].performed += OnReload;

    }

    private void OnEnable()
    {
        UpdateScriptableObject(weaponData);
    }

    private void OnDisable()
    {
        playerInput.actions["Fire"].performed -= OnFire;
        playerInput.actions["Reload"].performed -= OnReload;
    }

    private void Update()
    {
        if(weaponData.bulletsLoaded <= 0f)
        {
            Reload();
            return;
        }

        // if player is holding down "fire" button, then attempt to shoot
        if (shootInput > 0 && weaponData.CheckAmmo())
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
        if (!weaponData.isReloading)
            StartCoroutine(weaponData.WaitReload());
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

        Debug.Log("SPAWN A BULLET!");

        if (bullet != null)
        {  

            Bullet projectile = bullet.GetComponent<Bullet>();

            // Resetting rotation before applying spread
            //projectile.transform.rotation = bulletSpawnPoint.rotation;

            // pass that bullet into the weaponData's fire function
            weaponData.Fire(projectile);

            projectile.UpdateWeaponData(weaponData);
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
}
