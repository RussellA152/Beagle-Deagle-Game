using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class GunWeapon : MonoBehaviour
{
    private PlayerInput playerInput;

    [Range(0, 1000)]
    public float damagePerHit;

    [Header("Fire Rate (Bullets Per Second)")]
    public float fireRate; // The number of bullets fired per second

    [Header("Ammo")]
    public int magazineSize; // how much ammo can this weapon hold for its total magazine?
    [HideInInspector]
    public int bulletsShot; // how much ammo has the player shot since the last reload or refill?
    [HideInInspector]
    public int bulletsLoaded; // how much ammo is currently in the magazine?
    [HideInInspector]
    public int ammoInReserve; // how much ammo is currently in capacity?
    public int maxAmmoInReserve; // how much ammo can this weapon hold for total capacity?
    public float totalReloadTime; // how long will this gun take to reload to full?

    [Header("Bullet Logic")]
    public Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)
    public Bullet bullet; // what bullet is spawned when shooting?

    [Header("Penetration")]
    [Range(1f,50f)]
    public int penetrationCount; // how many enemies can this gun's bullet pass through?


    private float shootInput; // input for shooting

    public bool actuallyShooting; // is the player shooting (i.e, not idle or reloading or just moving)

    [HideInInspector]
    public bool isReloading;


    private void Start()
    {
        bulletsShot = 0;
        bulletsLoaded = magazineSize;
        ammoInReserve = maxAmmoInReserve;
        isReloading = false;


        playerInput = PlayerManager.instance.GetPlayerInput();

        //// TEMPORARILY HERE
        playerInput.actions["Fire"].performed += OnFire;
        playerInput.actions["Reload"].performed += OnReload;

    }

    private void OnDisable()
    {
        playerInput.actions["Fire"].performed -= OnFire;
        playerInput.actions["Reload"].performed -= OnReload;
    }

    private void Update()
    {
        // if player is holding down "fire" button, then attempt to shoot
        if (shootInput > 0 && CheckIfCanShoot())
            Fire();
        else
            actuallyShooting = false;
    }

    public abstract void Fire();

    public virtual void SpawnBullet()
    {
        // Kind of unoptimized?
        GameObject bulletInst = Instantiate(bullet.gameObject, bulletSpawnPoint.position, this.transform.rotation);
        bulletInst.GetComponent<Bullet>().SetGun(this);

        bulletsShot++;
        bulletsLoaded--;
    }

    public void OnFire(CallbackContext context)
    {
        shootInput = context.ReadValue<float>();

    }
    private void OnDrawGizmos()
    {
        // draws a green ray so its easier to aim (debugging purposes)
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector2.right) * 6;
        Gizmos.DrawRay(bulletSpawnPoint.position, direction);
    }
    #region Reloading

    public virtual bool CheckIfCanShoot()
    {
        // if player has no ammo in reserve... force them to reload, then return false
        if (bulletsLoaded <= 0f)
        {
            Debug.Log("RELOADING!");
            Reload();
            return false;
        }
        // otherwise if they are not reloading, allow them to shoot
        else if (!isReloading)
        {
            actuallyShooting = true;
            return true;
        }

        return false;
    }

    public void OnReload(CallbackContext context) {
        Reload();
    }
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
        actuallyShooting = false;
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
