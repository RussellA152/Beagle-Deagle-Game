using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon")]
public abstract class GunData : ScriptableObject
{
    public Sprite sprite;

    [Header("Damage")]
    [Range(0, 1000)]
    public float damagePerHit;

    [Header("Fire Rate (Bullets Per Second)")]
    public float fireRate; // The number of bullets fired per second

    [Header("Ammo")]
    public int magazineSize; // how much ammo can this weapon hold for its total magazine?

    //public int maxAmmoInReserve; // how much ammo can this weapon hold for total capacity?
    public float totalReloadTime; // how long will this gun take to reload to full?

    [Header("Bullet To Shoot")]
    public GameObject bullet; // what bullet is spawned when shooting?
    public ProjectileData bulletData; // what data will this bullet use?

    [Header("Weapon Spread")]
    [Range(0f, 20f)]
    public float spread; // spread of bullet in X direction

    [Header("Penetration")]
    [Range(1f, 50f)]
    public int penetrationCount; // how many enemies can this gun's bullet pass through?

    #region hiddenProperties

    [HideInInspector]
    public int bulletsShot; // how much ammo has the player shot since the last reload or refill?
    [HideInInspector]
    public int bulletsLoaded; // how much ammo is currently in the magazine?

    [HideInInspector]
    public Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)

    //public float lastTimeShot;

    //[HideInInspector]
    //public int ammoInReserve; // how much ammo is currently in capacity?

    [HideInInspector]
    public bool actuallyShooting; // is the player shooting (i.e, not idle or reloading or just moving)

    [HideInInspector]
    public bool isReloading;

    #endregion


    public virtual void OnEnable()
    {
        // always reset these values OnEnable because scriptable object data can persist *
        bulletsShot = 0;
        bulletsLoaded = magazineSize;
        isReloading = false;
        actuallyShooting = false;

        //ammoInReserve = weaponData.maxAmmoInReserve;
    }


    public abstract void Fire(Bullet bullet);

    public abstract bool CheckIfCanFire();


    public virtual bool CheckAmmo()
    {
        // if player has no ammo in reserve... force them to reload, then return false
        if (bulletsLoaded <= 0f)
        {
            Debug.Log("RELOADING!");
            return false;
        }
        // otherwise if they are not reloading, allow them to shoot
        else if (!isReloading && CheckIfCanFire())
        {

            actuallyShooting = true;
            return true;
        }

        return false;
    }

    public virtual void SpawnBullet(Bullet bullet, Transform spawnPoint)
    {

        if (bullet != null)
        {
            //lastTimeShot = 0f;
            bullet.UpdateProjectileData(bulletData);

            // set the position to be at the barrel of the gun
            bullet.transform.position = spawnPoint.position;
            
            // Apply the spread to the bullet's rotation
            bullet.transform.rotation = CalculateWeaponSpread(spawnPoint.rotation);

            bullet.gameObject.SetActive(true);

            bulletsShot++;
            bulletsLoaded--;
        }

    }

    // very simple weapon spread, just add a random offset to the bullet's Y position
    public virtual Quaternion CalculateWeaponSpread(Quaternion spawnPointRotation)
    {
        // Calculate the spread angle
        float spreadAngle = Random.Range(-spread, spread);

        return Quaternion.Euler(0f, 0f, spreadAngle) * spawnPointRotation;
    }

    #region Reloading
    public virtual IEnumerator WaitReload()
    {
        actuallyShooting = false;
        isReloading = true;

        yield return new WaitForSeconds(totalReloadTime);

        RefillAmmo();

        isReloading = false;
    }

    public virtual void RefillAmmo()
    {
        bulletsLoaded += bulletsShot;
        bulletsShot = 0;
    }

    // Gives player full ammo
    //public void FullAmmo()
    //{
    //    ammoInReserve = weaponData.maxAmmoInReserve;
    //    bulletsLoaded = weaponData.magazineSize;
    //    bulletsShot = 0;
    //}

    #endregion
}
