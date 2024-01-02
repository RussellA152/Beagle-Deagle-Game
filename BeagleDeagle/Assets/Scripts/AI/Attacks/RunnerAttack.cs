using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RunnerAttack : AIAttack<RunnerEnemyData>
{
    // All hitBoxes on this enemy
    private BoxCollider2D[] _hitBoxes;
    
    // A list of all colliders that weres damaged by this enemy's attack
    private List<Collider2D> _collidersDamaged = new List<Collider2D>();

    private AudioClipPlayer _audioClipPlayer;
    private CameraShaker _cameraShaker;

    // Particle effects to play when attack lands
    [SerializeField, RestrictedPrefab(typeof(PoolableParticle))] private GameObject damageParticleEffect;
    private PoolableParticle _poolableParticle;
    private int _poolableParticleKey;

    protected override void Start()
    {
        base.Start();
        
        // Grab all hitBoxes inside of the enemy
        _hitBoxes = GetComponentsInChildren<BoxCollider2D>();

        _audioClipPlayer = GetComponent<AudioClipPlayer>();
        _cameraShaker = GetComponent<CameraShaker>();
        
        // Get particle effect
        _poolableParticleKey = damageParticleEffect.GetComponent<IPoolable>().PoolKey;
        _poolableParticle = ObjectPooler.Instance.GetPooledObject(_poolableParticleKey).GetComponent<PoolableParticle>();
    }

    public override void InitiateAttack()
    {
        // For each hitBox on the enemy, check if any of them collided with something
        // We check this in the Update() of AIController
        foreach (BoxCollider2D hitBox in _hitBoxes)
        {
            CheckHitBox(hitBox);
            
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Check what the enemy's hitbox collided with, and then try to apply
    /// damage those gameObjects
    /// 
    private void CheckHitBox(Collider2D hitBox)
    {
        Collider2D[] collidersToDamage = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        
        // Only collide with what this enemy can attack
        filter.SetLayerMask(enemyScriptableObject.whatHitBoxDamages);

        int colliderCount = Physics2D.OverlapCollider(hitBox, filter, collidersToDamage);

        // For each enemy this hitBox collided with
        for (int i = 0; i < colliderCount; i++)
        {
            // If this collider was not already damaged by the enemy, then apply damage
            if (!_collidersDamaged.Contains(collidersToDamage[i]))
            {
                _collidersDamaged.Add(collidersToDamage[i]);
                collidersToDamage[i].gameObject.GetComponent<IHealth>().ModifyHealth(-1f * enemyScriptableObject.attackDamage);
                
                
                // Play a random melee attack sound
                _audioClipPlayer.PlayRandomGeneralAudioClip(enemyScriptableObject.attackLandedSounds, enemyScriptableObject.volume);
                
                // Play damage effect on the target
                if (_poolableParticle != null)
                {
                    _poolableParticle.PlaceParticleOnTransform(hitBox.transform);
                    _poolableParticle.PlayAllParticles(hitBox.transform.localScale.x);
                }

                if (_cameraShaker != null)
                {
                    _cameraShaker.ShakePlayerCamera(enemyScriptableObject.screenShakeData);
                }
                
            }
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// When the enemy's attack animation ends, reset all colliders damaged
    /// 
    public void ResetCollidersDamaged()
    {
        _collidersDamaged.Clear();
    }
    
}
