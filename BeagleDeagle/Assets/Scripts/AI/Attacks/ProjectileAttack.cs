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
        _bulletPoolKey = enemyScriptableObject.bulletPrefab.GetComponent<IPoolable>().PoolKey;
    }

    public override void InitiateAttack()
    {
        // The spawn point of all projectiles is always looking at the target
        projectileSpawnPoint.transform.right = (Vector2)Target.position - (Vector2)transform.position;
        
        // Fetch a bullet from the object pooler
        GameObject newBullet = ObjectPooler.Instance.GetPooledObject(_bulletPoolKey);
        
        if (newBullet != null)
        {
            IBulletUpdatable projectile = newBullet.GetComponent<IBulletUpdatable>();
            
            projectile.UpdateScriptableObject(enemyScriptableObject.bulletData);
            
            
            // Update bullet's status effects with data, only if this enemy's bullets has status effects
            if (enemyScriptableObject.statusEffects != null)
            {
                foreach (IStatusEffect statusEffect in newBullet.GetComponents<IStatusEffect>())
                {
                    Debug.Log("update!");
                    statusEffect.UpdateWeaponType(enemyScriptableObject.statusEffects);
                }
            }

            // Pass in the damage and penetration values of this gun, to the bullet being shot
            // Also account for any modifications to the gun damage and penetration (e.g, an item purchased by trader that increases player gun damage)
            projectile.UpdateDamage(enemyScriptableObject.attackDamage);
            projectile.UpdatePenetration(enemyScriptableObject.bulletPenetration);

            projectile.UpdateWhoShotThisBullet(transform);

            // Set the position to be at the barrel of the gun
            newBullet.transform.position = projectileSpawnPoint.position;
            newBullet.transform.rotation = projectileSpawnPoint.rotation;
            
            newBullet.gameObject.SetActive(true);
            
            projectile.ActivateBullet();
        }
        
        //BeginCooldown();
    }
}
