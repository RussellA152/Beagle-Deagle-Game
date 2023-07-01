using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunData : ScriptableObject
{
    public Sprite sprite;

    [Header("Fire Rate (Bullets Per Second)")]
    public float fireRate; // The number of bullets fired per second

    [Header("Ammo")]
    public int magazineSize; // how much ammo can this weapon hold for its total magazine?
    
    [Header("Reloading")]
    [SerializeField]
    [Range(0f, 30f)]
    private float totalReloadTime;
    
    [Header("Bullet To Shoot")]
    public GameObject bullet; // what bullet is spawned when shooting?
    public BulletData bulletData; // what data will this bullet use?

    [Header("Weapon Spread")]
    [Range(0f, 20f)]
    public float bulletSpread; // spread of bullet in X direction

    [Header("Penetration")]
    [Range(1f, 50f)]
    public int penetrationCount; // how many enemies can this gun's bullet pass through?
    
    [HideInInspector]
    public Transform bulletSpawnPoint; // where does this bullet get shot from? (i.e the barrel)
    
    protected int BulletPoolKey;
    
    public virtual void OnEnable()
    {
        BulletPoolKey = bullet.GetComponent<IPoolable>().PoolKey;
    }


    public int Fire(ObjectPooler bulletPool, float damageModifier, float spreadModifier, int penetrationModifier)
    {
        int bulletsShot = 0;
        
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

            return bulletsShot;

        }
        Debug.Log("Could not retrieve a bullet from object pool!");
        return 0;
    }

    public bool CheckIfCanFire(float lastTimeFired, float fireRateModifier)
    {
        return Time.time - lastTimeFired > 1f / fireRate * fireRateModifier;
    }


    public bool CheckAmmo(int ammoLoaded)
    {
        // If player has no ammo in reserve,
        // then force them to reload by returning false
        return ammoLoaded > 0f;
    }

    ///-///////////////////////////////////////////////////////////
    /// Add a random offset to the bullet's Y position
    /// to simulate random spread.
    public virtual Quaternion CalculateWeaponSpread(Quaternion spawnPointRotation, float spreadModifier)
    {
        // Calculate the spread angle
        float spreadAngle = Random.Range(-bulletSpread * spreadModifier, bulletSpread * spreadModifier);

        return Quaternion.Euler(0f, 0f, spreadAngle) * spawnPointRotation;
    }

    ///-///////////////////////////////////////////////////////////
    /// Return the damage of this weapon.
    /// 
    public abstract float GetDamage();

    #region Reloading

    public virtual IEnumerator WaitReload(float reloadTimeModifier)
    {
        yield return new WaitForSeconds(totalReloadTime * reloadTimeModifier);
    }

    ///-///////////////////////////////////////////////////////////
    /// When the gun is done reloading, refill all the ammo
    /// 
    public virtual void RefillAmmoCompletely(out int newBulletsLoaded, out int newBulletsShot)
    {
        newBulletsLoaded = magazineSize;
        newBulletsShot = 0;
    }

    #endregion
}
