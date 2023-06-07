using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    private BulletData bulletData;

    [SerializeField]
    private int poolKey;
    public int PoolKey => poolKey; // Return the pool key (anything that is IPoolable, must have a pool key)

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private CapsuleCollider2D bulletCollider; // The collider of this bullet

    private Vector3 defaultRotation = new Vector3(0f, 0f, -90f);

    private readonly HashSet<Transform> hitEnemies = new HashSet<Transform>(); // Don't let this bullet hit the same enemy twice (we track what this bullet hit in this HashSet)

    private float damagePerHit; // The damage of the player's gun or enemy that shot this bullet

    private int penetrationCount; // The amount of penetration of the player's gun or enemy that shot this bullet

    private int amountPenetrated; // How many enemies has this bullet penetrated through?

    private void OnEnable()
    {
        
        if (bulletData != null)
        {
            // Start time for this bullet to disable
            StartCoroutine(DisableAfterTime());

            UpdateWeaponValues(bulletData.bulletDamage, bulletData.bulletPenetration);
            
            // Change the bullet's collider size to whatever the scriptable object has
            bulletCollider.size = new Vector2(bulletData.sizeX, bulletData.sizeY);
            // Change the bullet's collider direction to whatever the scriptable object has
            bulletCollider.direction = bulletData.colliderDirection;

            // Apply the trajectory of this bullet (We probably could do this inside of the gameobject that spawns it?)
            bulletData.ApplyTrajectory(rb, transform);
            //Debug.Log("Was enabled!");
        }
    }

    private void OnDisable()
    {
        hitEnemies.Clear();

        // Resetting rotation before applying spread
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.Euler(defaultRotation);

        // Resetting capsule collider direction
        bulletCollider.direction = CapsuleDirection2D.Vertical;

        // Reset number of penetration
        amountPenetrated = 0;
        penetrationCount = 0;

        // Reset damage
        damagePerHit = 0;

        // Stop all coroutines when this bullet has been disabled
        StopAllCoroutines();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this bullet already hit this enemy, then don't allow penetration or damage to occur
        if (hitEnemies.Contains(collision.transform))
        {
            Debug.Log("Hit already!");
            return;
        }
            

        // If this bullet hits what its allowed to
        if ((bulletData.whatBulletCanPenetrate.value & (1 << collision.gameObject.layer)) > 0)
        {

            // Check if this bullet can damage that gameobject
            if((bulletData.whatBulletCanDamage.value & (1 << collision.gameObject.layer)) > 0)
            {
                // Add this enemy to the list of hitEnemies
                hitEnemies.Add(collision.transform);

                Debug.Log(damagePerHit);

                // Apply damage from both bullet and gun
                bulletData.OnHit(rb, collision.gameObject, damagePerHit);
            }
            
            // Penetrate through object
            Penetrate();
        }
    }

    // Call this function each time this bullet hits their target
    private void Penetrate()
    {
        amountPenetrated++;

        // If this bullet has penetrated through too many targets, then disable it
        if (amountPenetrated >= penetrationCount)
        {
            gameObject.SetActive(false);
        }

    }

    // Update the damage and penetration values
    public void UpdateWeaponValues(float damage, int penetration)
    {
        Debug.Log(damage);
        damagePerHit += damage;

        penetrationCount += penetration;
    }

    // Update the bullet with its scriptable object (Contains trajectory logic and any special ability. e.g, Incinerating on hit)
    public void UpdateProjectileData(BulletData scriptableObject)
    {
        bulletData = scriptableObject;
    }

    // If this bullet exists for too long, disable it
    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(bulletData.destroyTime);
        gameObject.SetActive(false);
    }

}
