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

    private float damagePerHit; // The damage of the player's gun or enemy that shot this bullet

    private int penetrationCount; // The amount of penetration of the player's gun or enemy that shot this bullet

    private int amountPenetrated; // How many enemies has this bullet penetrated through?

    private void OnEnable()
    {   
        // Reset number of penetration
        amountPenetrated = 0;

        if(bulletData != null)
        {
            // Start time for this bullet to disable
            StartCoroutine(DisableAfterTime());
            
            // Change the bullet's collider size to whatever the scriptable object has
            bulletCollider.size = new Vector2(bulletData.sizeX, bulletData.sizeY);
            // Change the bullet's collider direction to whatever the scriptable object has
            bulletCollider.direction = bulletData.colliderDirection;

            // Apply the trajectory of this bullet (We probably could do this inside of the gameobject that spawns it?)
            bulletData.ApplyTrajectory(rb, transform);
            Debug.Log("Was enabled!");
        }
    }

    private void OnDisable()
    {
        // Resetting rotation before applying spread
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.Euler(defaultRotation);

        // Resetting capsule collider direction
        bulletCollider.direction = CapsuleDirection2D.Vertical;

        // Stop all coroutines when this bullet has been disabled
        StopAllCoroutines();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to
        if ((bulletData.whatBulletCanPenetrate.value & (1 << collision.gameObject.layer)) > 0)
        {
            //Debug.Log("BULLET HIT " + collision.gameObject.name);

            // Check if this bullet can damage that gameobject
            if((bulletData.whatBulletCanDamage.value & (1 << collision.gameObject.layer)) > 0)
            {
                bulletData.OnHit(collision, damagePerHit);
            }
            
            // Penetrate through object
            Penetrate();
        }

        // If this bullet cannot penetrate a certain layer, do not allow collision or damage to occur
        //else
        //{
        //    Debug.Log("Bullet hit non-collidable layer!");
        //    gameObject.SetActive(false);
        //}
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
        damagePerHit = damage;
        penetrationCount = penetration;
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
