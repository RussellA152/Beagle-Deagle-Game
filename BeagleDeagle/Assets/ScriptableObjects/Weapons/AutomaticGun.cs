using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon/AutomaticGun")]
public class AutomaticGun : GunData
{
    private float lastFireTime;

    public override void OnEnable()
    {
        base.OnEnable();
        lastFireTime = 0f;
    }
    public override int Fire(ObjectPooler bulletPool, float damageModifier, float spreadModifier, int penetrationModifier)
    {
        GameObject bullet;

        // Fetch a bullet from object pooler
        bullet = bulletPool.GetPooledObject(bulletPoolKey);

        if (bullet != null)
        {

            Bullet projectile = bullet.GetComponent<Bullet>();

            // Pass in the damage and penetration values of this gun, to the bullet being shot
            // Also account for any modifications to the gun damage and penetration (e.g, an item purchased by trader that increases player gun damage)
            projectile.UpdateWeaponValues(damagePerHit * damageModifier, penetrationCount + penetrationModifier);

            // Giving the bullet its data (for the 'destroyTime' variable and 'trajectory' method)
            projectile.UpdateProjectileData(bulletData);

            // Set the position to be at the barrel of the gun
            bullet.transform.position = bulletSpawnPoint.position;

            // Apply the spread to the bullet's rotation
            bullet.transform.rotation = CalculateWeaponSpread(bulletSpawnPoint.rotation, bulletSpread * spreadModifier);

            bullet.gameObject.SetActive(true);

            bulletsShot++;
            bulletsLoaded--;

            lastFireTime = Time.time;

            return bulletsLoaded;

        }
        else
        {
            lastFireTime = Time.time;
            Debug.Log("Could not retrieve a bullet from object pool!");
            return 0;
        }

        

    }

    public override bool CheckIfCanFire(float fireRateModifier)
    {
        if (Time.time - lastFireTime > 1f / fireRate * fireRateModifier)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
}
