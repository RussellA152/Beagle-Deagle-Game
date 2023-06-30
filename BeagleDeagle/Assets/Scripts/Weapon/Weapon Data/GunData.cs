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


    public virtual int Fire(ObjectPooler bulletPool, float damageModifier, float spreadModifier, int penetrationModifier)
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

    public virtual bool CheckIfCanFire(float lastTimeFired, float fireRateModifier)
    {
        return Time.time - lastTimeFired > 1f / fireRate * fireRateModifier;
    }


    public virtual bool CheckAmmo(int ammoLoaded)
    {
        // if player has no ammo in reserve... force them to reload, then return false
        if (ammoLoaded <= 0f)
        {
            return false;
        }
        
        return true;
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
    public virtual void RefillAmmo(out int newBulletsLoaded, out int newBulletsShot)
    {
        newBulletsLoaded = magazineSize;
        newBulletsShot = 0;
    }

    #endregion
}
