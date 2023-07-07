using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet<T> : MonoBehaviour, IPoolable, IBulletUpdatable where T: BulletData
{
    protected T bulletData;

    [SerializeField]
    private int poolKey;
    public int PoolKey => poolKey; // Return the pool key (anything that is IPoolable, must have a pool key)

    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    protected CapsuleCollider2D bulletCollider; // The collider of this bullet

    private Vector3 defaultRotation = new Vector3(0f, 0f, -90f);

    private readonly HashSet<Transform> _hitEnemies = new HashSet<Transform>(); // Don't let this bullet hit the same enemy twice (we track what this bullet hit in this HashSet)

    private float _damagePerHit; // The damage of the player's gun or enemy that shot this bullet

    private int _penetrationCount; // The amount of penetration of the player's gun or enemy that shot this bullet

    private int _amountPenetrated; // How many enemies has this bullet penetrated through?
    
    [SerializeField]
    // What should bullet do (besides just damaging target..)
    private UnityEvent<GameObject> onBulletHit;

    private void OnEnable()
    {
        
        if (bulletData != null)
        {
            // Start time for this bullet to disable
            StartCoroutine(DisableAfterTime());

            //UpdateWeaponValues(bulletData.bulletDamage, bulletData.bulletPenetration);
            
            // Change the bullet's collider size to whatever the scriptable object has
            bulletCollider.size = new Vector2(bulletData.sizeX, bulletData.sizeY);
            // Change the bullet's collider direction to whatever the scriptable object has
            bulletCollider.direction = bulletData.colliderDirection;

            // Apply the trajectory of this bullet (We probably could do this inside of the gameobject that spawns it?)
            ApplyTrajectory(rb, transform);
            //Debug.Log("Was enabled!");
        }
    }

    private void OnDisable()
    {
        _hitEnemies.Clear();

        // Resetting rotation before applying spread
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.Euler(defaultRotation);

        // Resetting capsule collider direction
        bulletCollider.direction = CapsuleDirection2D.Vertical;

        // Reset number of penetration
        _amountPenetrated = 0;
        _penetrationCount = 0;

        // Reset damage
        _damagePerHit = 0;

        // Stop all coroutines when this bullet has been disabled
        StopAllCoroutines();
    }

    private void ApplyTrajectory(Rigidbody2D rb, Transform transform)
    {
        rb.velocity = transform.right * bulletData.bulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        onBulletHit.Invoke(collision.gameObject);
        
        // If this bullet already hit this enemy, then don't allow penetration or damage to occur
        if (_hitEnemies.Contains(collision.transform))
        {
            Debug.Log("Hit already!");
            return;
        }
        
        // If this bullet hits something that destroys it, disable the bullet
        if((bulletData.whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            gameObject.SetActive(false);
            return;
        }
            

        // If this bullet hits what its allowed to
        if ((bulletData.whatBulletCanPenetrate.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Check if this bullet can damage that gameObject
            if((bulletData.whatBulletCanDamage.value & (1 << collision.gameObject.layer)) > 0)
            {
                // Add this enemy to the list of hitEnemies
                _hitEnemies.Add(collision.transform);

                Debug.Log(_damagePerHit);

                DamageOnHit(collision.gameObject);
            }
            // Penetrate through object
            Penetrate();
        }
    }

    protected virtual void DamageOnHit(GameObject objectHit)
    {
        objectHit.GetComponent<IHealth>().ModifyHealth(-1 * _damagePerHit);
    }

    // Call this function each time this bullet hits their target
    private void Penetrate()
    {
        _amountPenetrated++;

        // If this bullet has penetrated through too many targets, then disable it
        if (_amountPenetrated >= _penetrationCount)
        {
            gameObject.SetActive(false);
        }

    }
    
    // Update the damage and penetration values
    public void UpdateWeaponValues(float damage, int penetration)
    {
        //Debug.Log(damage);
        _damagePerHit += damage;

        _penetrationCount += penetration;
    }
    
    // If this bullet exists for too long, disable it
    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(bulletData.GetLifeTime());
        gameObject.SetActive(false);
    }

    public void UpdateScriptableObject(BulletData scriptableObject)
    {
        if (scriptableObject is T)
        {
            bulletData = scriptableObject as T;
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }
}

