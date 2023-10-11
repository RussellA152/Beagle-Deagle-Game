using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Bullet<T> : MonoBehaviour, IPoolable, IBulletUpdatable where T: BulletData
{
    protected T bulletData;

    [SerializeField]
    private int poolKey;
    public int PoolKey => poolKey; // Return the pool key (anything that is IPoolable, must have a pool key)

    protected Rigidbody2D rb;
    protected CapsuleCollider2D bulletCollider; // The collider of this bullet
    private SpriteRenderer _spriteRenderer;

    private Vector3 defaultRotation = new Vector3(0f, 0f, -90f);
    
    // Who shot this bullet? (usually player or enemy)
    protected Transform _whoShotThisBullet;

    private readonly HashSet<Transform> _hitEnemies = new HashSet<Transform>(); // Don't let this bullet hit the same enemy twice (we track what this bullet hit in this HashSet)

    private float _damagePerHit; // The damage of the player's gun or enemy that shot this bullet

    private int _penetrationCount; // The amount of penetration of the player's gun or enemy that shot this bullet

    private int _amountPenetrated; // How many enemies has this bullet penetrated through?
    
    [SerializeField]
    // What should bullet do (besides just damaging target..)
    private UnityEvent<GameObject> onBulletHit;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        if (bulletData != null)
        {
            // Start time for this bullet to disable
            StartCoroutine(DisableAfterTime());
            
            // Change the bullet's transform scale to whatever the scriptable object has
            transform.localScale = new Vector2(bulletData.sizeX, bulletData.sizeY);
            
            //bulletCollider.size = new Vector2(bulletData.sizeX, bulletData.sizeY);
            
            // Change the bullet's collider direction to whatever the scriptable object has
            bulletCollider.direction = bulletData.colliderDirection;

            // Apply the trajectory of this bullet (We probably could do this inside of the gameobject that spawns it?)
            ApplyTrajectory();
        }
    }

    protected virtual void OnDisable()
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

    private void ApplyTrajectory()
    {
        rb.velocity = transform.right * bulletData.bulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

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
        
        onBulletHit.Invoke(collision.gameObject);
        Debug.Log(gameObject + " HIT " +  collision.gameObject);
            

        // If this bullet hits what its allowed to
        if ((bulletData.whatBulletCanPenetrate.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Check if this bullet can damage that gameObject
            if((bulletData.whatBulletCanDamage.value & (1 << collision.gameObject.layer)) > 0)
            {
                // Add this enemy to the list of hitEnemies
                _hitEnemies.Add(collision.transform);

                Debug.Log(_damagePerHit + " applied to " + collision.gameObject);

                DamageOnHit(collision.gameObject);
                
                Vector2 knockBackDirection = rb.velocity.normalized;

                // If the target is knockBack-able
                if (collision.gameObject.GetComponent<IKnockBackable>() != null)
                {
                    collision.gameObject.GetComponent<IKnockBackable>().ApplyKnockBack(knockBackDirection, bulletData.knockBackPower);
                }
            }
            // Penetrate through object
            Penetrate();
        }
    }

    protected virtual void DamageOnHit(GameObject objectHit)
    {
        IHealth healthScript = objectHit.GetComponent<IHealth>();
        
        healthScript?.ModifyHealth(-1 * _damagePerHit);
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
    public void UpdateDamageAndPenetrationValues(float damage, int penetration)
    {
        _damagePerHit += damage;

        _penetrationCount += penetration;
        
    }

    public void UpdateWhoShotThisBullet(Transform shooter)
    {
        _whoShotThisBullet = shooter;
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
            // Update bullet appearance
            if(bulletData.bulletSprite != null)
            {
                _spriteRenderer.sprite = bulletData.bulletSprite;
            }
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }
}

