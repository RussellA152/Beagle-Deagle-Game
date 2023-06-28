using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon/Gun")]
public abstract class GunData : ScriptableObject
{
    public Sprite sprite;

    [Header("Fire Rate (Bullets Per Second)")]
    public float fireRate; // The number of bullets fired per second

    [Header("Ammo")]
    public int magazineSize; // how much ammo can this weapon hold for its total magazine?

    //public int maxAmmoInReserve; // how much ammo can this weapon hold for total capacity?
    //public float totalReloadTime; // how long will this gun take to reload to full?

    [Header("Bullet To Shoot")]
    public GameObject bullet; // what bullet is spawned when shooting?
    public BulletData bulletData; // what data will this bullet use?

    [Header("Weapon Spread")]
    [Range(0f, 20f)]
    public float bulletSpread; // spread of bullet in X direction

    [Header("Penetration")]
    [Range(1f, 50f)]
    public int penetrationCount; // how many enemies can this gun's bullet pass through?

    private float _lastFireTime;

    #region hiddenProperties

    [HideInInspector]
    public int bulletsShot; // how much ammo has the player shot since the last reload or refill?
    [HideInInspector]
    public int bulletsLoaded; // how much ammo is currently in the magazine?

    [HideInInspector]
    public Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)

    [HideInInspector]
    public bool actuallyShooting; // is the player shooting (i.e, not idle or reloading or just moving)

    [HideInInspector]
    public bool isReloading;

    protected int BulletPoolKey;

    #endregion


    public virtual void OnEnable()
    {
        // Always reset these values OnEnable because scriptable object data can persist *
        bulletsShot = 0;
        bulletsLoaded = magazineSize;
        isReloading = false;
        actuallyShooting = false;

        BulletPoolKey = bullet.GetComponent<IPoolable>().PoolKey;

        _lastFireTime = 0f;
    }


    public virtual int Fire(ObjectPooler bulletPool, float damageModifier, float spreadModifier, int penetrationModifier)
    {
        //Debug.Log("Fired as: " + this.name);

        // Fetch a bullet from object pooler
        GameObject newBullet = bulletPool.GetPooledObject(BulletPoolKey);

        if (newBullet != null)
        {

            Bullet projectile = newBullet.GetComponent<Bullet>();

            // Pass in the damage and penetration values of this gun, to the bullet being shot
            // Also account for any modifications to the gun damage and penetration (e.g, an item purchased by trader that increases player gun damage)
            projectile.UpdateWeaponValues(GetDamage() * damageModifier, penetrationCount + penetrationModifier);

            // Giving the bullet its data (for the 'destroyTime' variable and 'trajectory' method)
            projectile.UpdateProjectileData(bulletData);

            // Set the position to be at the barrel of the gun
            newBullet.transform.position = bulletSpawnPoint.position;

            // Apply the spread to the bullet's rotation
            newBullet.transform.rotation = CalculateWeaponSpread(bulletSpawnPoint.rotation, bulletSpread * spreadModifier);

            newBullet.gameObject.SetActive(true);

            bulletsShot++;
            bulletsLoaded--;

            _lastFireTime = Time.time;

            return bulletsLoaded;

        }
        else
        {
            _lastFireTime = Time.time;
            Debug.Log("Could not retrieve a bullet from object pool!");
            return 0;
        }
    }

    public virtual bool CheckIfCanFire(float fireRateModifier)
    {
        return Time.time - _lastFireTime > 1f / fireRate * fireRateModifier;
    }


    public virtual bool CheckAmmo()
    {
        // if player has no ammo in reserve... force them to reload, then return false
        if (bulletsLoaded <= 0f)
        {
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


    // very simple weapon spread, just add a random offset to the bullet's Y position
    public virtual Quaternion CalculateWeaponSpread(Quaternion spawnPointRotation, float spreadModifier)
    {
        // Calculate the spread angle
        float spreadAngle = Random.Range(-bulletSpread * spreadModifier, bulletSpread * spreadModifier);

        return Quaternion.Euler(0f, 0f, spreadAngle) * spawnPointRotation;
    }

    public abstract float GetDamage();

    #region Reloading
    public abstract IEnumerator WaitReload(float reloadTimeModifier);
    
    // When the gun is done reloading, refill all the ammo
    // For pump-action shotguns, we might only refill 1 bullet at a time
    public virtual void RefillAmmo()
    {
        bulletsLoaded += bulletsShot;
        bulletsShot = 0;
    }

    #endregion
}
