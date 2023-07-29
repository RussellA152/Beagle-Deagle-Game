using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : AIAttack<ProjectileEnemyData>
{
    // Where does this enemy's bullet spawn from?
    [SerializeField] private Transform projectileSpawnPoint;

    private int _bulletPoolKey;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        // TODO: See if we can call this function less?
        _bulletPoolKey = enemyScriptableObject.bulletType.bulletPrefab.GetComponent<IPoolable>().PoolKey;
    }

    public override void InitiateAttack()
    {
        // The spawn point of all projectiles is always looking at the target
        projectileSpawnPoint.transform.right = (Vector2)Target.position - (Vector2)transform.position;
        
        
        GameObject newBullet = ObjectPooler.instance.GetPooledObject(_bulletPoolKey);
        
        
        if (newBullet != null)
        {
            // Tell BulletType to update the bullet with the data it needs
            // For example, give fire damage to the bullet, or give life steal values to the bullet
            // Pass in the bullet gameObject and the player gameObject(to retrieve modifiers)
            IBulletUpdatable projectile = enemyScriptableObject.bulletType.UpdateBulletWithData(newBullet, transform.gameObject);
            
            // Pass in the damage and penetration values of this gun, to the bullet being shot
            // Also account for any modifications to the gun damage and penetration (e.g, an item purchased by trader that increases player gun damage)
            projectile.UpdateDamageAndPenetrationValues(enemyScriptableObject.attackDamage, enemyScriptableObject.bulletPenetration);

            // Set the position to be at the barrel of the gun
            newBullet.transform.position = projectileSpawnPoint.position;

            newBullet.transform.rotation = projectileSpawnPoint.rotation;


            newBullet.gameObject.SetActive(true);
            
        }

    }
}
