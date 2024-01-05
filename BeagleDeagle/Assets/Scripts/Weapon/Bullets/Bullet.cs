using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Bullet<T> : MonoBehaviour, IPoolable, IBulletUpdatable where T: BulletData
{
    protected T bulletData;

    [SerializeField] private int poolKey;
    public int PoolKey => poolKey; // Return the pool key (anything that is IPoolable, must have a pool key)
    
    [SerializeField] private bool shouldDestroy; // When hitting too many enemies or after a few seconds, should this bullet destroy or get disabled?(use object pool or not)

    protected Rigidbody2D rb;
    protected CapsuleCollider2D bulletCollider; // The collider of this bullet
    private SpriteRenderer _spriteRenderer;

    protected Vector3 DefaultRotation = new Vector3(0f, 0f, -90f);
    
    protected Transform _whoShotThisBullet; // Who shot this bullet? (usually player or enemy)
    
    [SerializeField] private bool allowMultipleHitsOnSameEnemy = false; // False by default, but should be true for projectiles that orbit or ricochet
    private readonly HashSet<Transform> _hitEnemies = new HashSet<Transform>(); // Don't let this bullet hit the same enemy twice (we track what this bullet hit in this HashSet)

    protected float DamagePerHit; // The damage of the player's gun or enemy that shot this bullet

    [Header("Penetration")]
    private int _penetrationCount; // The amount of penetration of the player's gun or enemy that shot this bullet
    private int _amountPenetrated; // How many enemies has this bullet penetrated through?


    [Header("Impact Particle Effects")]
    [SerializeField] private LayerMask _inanimateParticleLayerMask; // When hitting inanimate object, instantiate a particle effect on it (typically a spark)
    [SerializeField] private GameObject inanimateObjectHitParticleEffect;
    private int _inanimateHitParticlePoolKey;
    [SerializeField] private LayerMask _enemyParticleLayerMask; // When hitting a player or enemy, instantiate a particle effect on them (typically a blood particle effect)
    [SerializeField] private GameObject enemyHitParticleEffect;
    private int _enemyHitParticlePoolKey;

    protected AudioClipPlayer AudioClipPlayer;
    
    [SerializeField]
    // What should bullet do (besides just damaging target..)
    private UnityEvent<GameObject> onBulletHit;

    protected virtual void Awake()
    {
        // Cache dependencies
        rb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        AudioClipPlayer = GetComponent<AudioClipPlayer>();
        
        if(enemyHitParticleEffect != null)
            _enemyHitParticlePoolKey = enemyHitParticleEffect.GetComponent<IPoolable>().PoolKey;
        
        if(inanimateObjectHitParticleEffect != null)
            _inanimateHitParticlePoolKey = inanimateObjectHitParticleEffect.GetComponent<IPoolable>().PoolKey;
    }

    protected virtual void OnEnable()
    {
        
    }


    protected virtual void OnDisable()
    {
        _hitEnemies.Clear();

        // Resetting rotation before applying spread
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.Euler(DefaultRotation);

        // Resetting capsule collider direction
        bulletCollider.direction = CapsuleDirection2D.Vertical;

        // Reset number of penetration
        _amountPenetrated = 0;
        _penetrationCount = 0;

        // Reset damage
        DamagePerHit = 0;

        // Stop all coroutines when this bullet has been disabled
        StopAllCoroutines();
    }

    public virtual void ActivateBullet()
    {
        if (bulletData != null)
        {
            // Start time for this bullet to disable
            StartCoroutine(DisableAfterTime());
            
            // Change the size of the bullet to whatever the BulletData contains
            UpdateBulletSize();
            
            // Change the bullet's collider direction to whatever the scriptable object has
            bulletCollider.direction = bulletData.colliderDirection;

            // Apply the trajectory of this bullet
            ApplyTrajectory();
        }
    }

    protected virtual void ApplyTrajectory()
    {
        rb.velocity = transform.right * bulletData.bulletSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // If this bullet already hit this enemy (and is not allowed to hit the same enemy more than once),
        // then don't allow penetration or damage to occur
        if (!allowMultipleHitsOnSameEnemy && _hitEnemies.Contains(collision.transform))
        {
            return;
        }
        
        // If this bullet hits something that destroys it, disable the bullet
        if((bulletData.whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            
            ActivateParticleEffect(collision.gameObject);
            
            PlayDamageSound(true);
            
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
                
                DamageOnHit(collision.gameObject);
                
                PlayDamageSound(false);
                
                // Play particle effect on enemy hit
                ActivateParticleEffect(collision.gameObject);
                
                Vector2 knockBackDirection = rb.velocity.normalized;

                IKnockbackable knockbackableScript = collision.gameObject.GetComponent<IKnockbackable>();
                
                // If the target is knockback-able
                if (knockbackableScript != null)
                {
                    knockbackableScript.ApplyKnockBack(knockBackDirection, bulletData.knockBackPower);
                }
            }
            
            // Penetrate through objects
            Penetrate();
        }
    }

    protected virtual void DamageOnHit(GameObject objectHit)
    {
        IHealth healthScript = objectHit.GetComponent<IHealth>();

        healthScript?.ModifyHealth(-1 * DamagePerHit);
        
        onBulletHit.Invoke(objectHit);
        
    }

    private void ActivateParticleEffect(GameObject objectHit)
    {
        GameObject hitParticleEffect;
        PoolableParticle particleUsed;

        if ((_enemyParticleLayerMask.value & (1 << objectHit.layer)) > 0)
        {
            // Spawn a blood particle effect
            hitParticleEffect =  ObjectPooler.Instance.GetPooledObject(_enemyHitParticlePoolKey);
            particleUsed = hitParticleEffect.GetComponent<PoolableParticle>();
            particleUsed.PlaceParticleOnTransform(objectHit.transform);
            
            particleUsed.PlayAllParticles(1f);
        }
        else if((_inanimateParticleLayerMask.value & (1 << objectHit.layer)) > 0)
        {
            hitParticleEffect = ObjectPooler.Instance.GetPooledObject(_inanimateHitParticlePoolKey);
            particleUsed = hitParticleEffect.GetComponent<PoolableParticle>();
            particleUsed.PlaceParticleOnTransform(transform);
            
            particleUsed.PlayAllParticles(1f);
        }
        

    }

    // Call this function each time this bullet hits their target
    private void Penetrate()
    {
        _amountPenetrated++;

        // If this bullet has penetrated through too many targets, then disable it
        if (_amountPenetrated >= _penetrationCount)
        {
            if(shouldDestroy)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }

    }

    private void PlayDamageSound(bool isInanimate)
    {
        if (isInanimate)
        {
            AudioClipPlayer.PlayRandomGeneralAudioClip(bulletData.hitWallSound, bulletData.volume);
        }
        else
        {
            AudioClipPlayer.PlayRandomGeneralAudioClip(bulletData.hitBodySounds, bulletData.volume);
        }
    }
    

    ///-///////////////////////////////////////////////////////////
    /// Use BulletData scriptable object to adjust the local scale of a bullet.
    /// 
    protected virtual void UpdateBulletSize()
    {
        // Change the bullet's transform scale to whatever the scriptable object has
        transform.localScale = new Vector2(bulletData.sizeX, bulletData.sizeY);
    }
    
    // Update the damage and penetration values
    public void UpdateDamageAndPenetrationValues(float damage, int penetration)
    {
        DamagePerHit += damage;

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
        
        if(shouldDestroy)
            Destroy(gameObject);
        else
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