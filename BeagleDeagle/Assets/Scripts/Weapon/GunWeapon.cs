using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class GunWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private PlayerInput playerInput;

    [Range(0, 1000)]
    public float damage;

    [Header("Fire Rate")]
    public float fireRate; // The number of bullets fired per second

    [Header("Ammo")]
    public int magazineSize;
    [HideInInspector]
    public int ammoInReserve;
    [HideInInspector]
    public int bulletsShot;
    [HideInInspector]
    public int bulletsLoaded;
    public int maxAmmoInReserve;
    public float totalReloadTime; // how long will this gun take to reload to full?

    [Header("Bullet")]
    public float bulletTravelDistance; // how far until this bullet despawns
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    [Header("Penetration")]
    public int penetrationCount; // how many enemies can this gun's bullet pass through?


    private float shooting;

    [HideInInspector]
    public bool isReloading;

    private void Start()
    {
        bulletsShot = 0;
        bulletsLoaded = magazineSize;
        ammoInReserve = maxAmmoInReserve;
        isReloading = false;

        playerInput = player.GetComponent<PlayerInput>();
    }

    public void OnFire(CallbackContext context)
    {
        shooting = context.ReadValue<float>();

        if(shooting > 0)
            FireRate();

    }
    public virtual void Shoot()
    {
        // if player has no ammo in reserve, force them to reload
        if (bulletsLoaded <= 0f)
        {
            Debug.Log("RELOADING!");
            Reload();
        }

        else if (!isReloading)
        {
            Debug.Log("SHOOT!");

            SpawnBullet();
            bulletsShot++;
            bulletsLoaded--;
        }
    }
    public abstract void FireRate();

    public virtual void SpawnBullet()
    {
        GameObject bulletInst = Instantiate(bulletPrefab, bulletSpawnPoint.position, this.transform.rotation);
    }


    #region Reloading
    public void Reload()
    {
        // dont' reload if player doesn't have ammo in reserve
        // or, if the player has a full magazine clip
        if (ammoInReserve == 0 || bulletsLoaded == magazineSize)
            return;

        if (!isReloading)
            StartCoroutine(WaitReload());
    }

    public virtual IEnumerator WaitReload()
    {
        isReloading = true;

        yield return new WaitForSeconds(totalReloadTime);

        RefillAmmo();

        isReloading = false;
    }

    // Take away ammo from player reserves after they finished reloading
    // This way of refilling ammo should work for all weapons except for pump shotguns
    public virtual void RefillAmmo()
    {
        // has ammo in reserve after reloading (ex. 7/56 ammo to 8/55 ammo)
        if((ammoInReserve - bulletsShot) > 0)
        {
            ammoInReserve -= bulletsShot;
            bulletsLoaded += bulletsShot;
            bulletsShot = 0;
        }
        // does not have ammo in reserve after reloading (ex. 6/2 to 8/0)
        else if((ammoInReserve - bulletsShot) <= 0)
        {
            bulletsLoaded += ammoInReserve;
            ammoInReserve = 0;
            bulletsShot = 0;
        }
    }

    // Gives player full ammo
    public void FullAmmo()
    {
        ammoInReserve = maxAmmoInReserve;
        bulletsLoaded = magazineSize;
        bulletsShot = 0;
    }
    #endregion
}
