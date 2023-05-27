using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    private BulletData bulletData;

    [SerializeField]
    private int poolKey;

    public int PoolKey => poolKey; // return the pool key (anything that is IPoolable, must have a pool key)

    [SerializeField]
    private Rigidbody2D rb;

    private Vector3 defaultRotation = new Vector3(0f, 0f, -90f);

    private float damagePerHit; // the damage of the player's gun or enemy that shot this bullet

    private int penetrationCount; // the amount of penetration of the player's gun or enemy that shot this bullet

    private int amountPenetrated; // how many enemies has this bullet penetrated through?

    private void OnEnable()
    {   
        // Reset number of penetration
        amountPenetrated = 0;

        if(bulletData != null)
        {
            // Start time for this bullet to disable
            StartCoroutine(DisableAfterTime());

            // Apply the trajectory of this bullet (We probably could do this inside of the gameobject that spawns it?)
            bulletData.ApplyTrajectory(rb, transform);
        }
    }

    private void OnDisable()
    {
        // Resetting rotation before applying spread
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.Euler(defaultRotation);

        // stop all coroutines when this bullet has been disabled
        StopAllCoroutines();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((bulletData.whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log("BULLET HIT " + collision.gameObject.name);

            // Make target take damage
            collision.gameObject.GetComponent<IHealth>().ModifyHealth(-1 * damagePerHit);


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
