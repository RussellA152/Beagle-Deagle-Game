using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Bullet<ExplosiveBulletData>
{
    private CameraShaker _cameraShaker;

    [SerializeField, RestrictedPrefab(typeof(PoolableParticle))] 
    private GameObject explosiveParticleGameObject;
    
    private PoolableParticle _particleEffectScript;
    private int _explosiveParticlePoolKey;
    
    private CheckObstruction _obstructionScript;

    protected override void Awake()
    {
        base.Awake();
        
        _cameraShaker = GetComponent<CameraShaker>();
        _obstructionScript = GetComponent<CheckObstruction>();
        
        // Find explosive effect if this gameObject needs one
        if(explosiveParticleGameObject != null)
            _explosiveParticlePoolKey = explosiveParticleGameObject.GetComponent<IPoolable>().PoolKey;
    }

    protected override void DamageOnHit(GameObject objectHit)
    {
        base.DamageOnHit(objectHit);
        
        PlayParticleEffect();
        
        // Play explosive sound
        AudioClipPlayer.PlayRandomGeneralAudioClip(bulletData.explosiveData.explosionClips, bulletData.explosiveData.explosiveSoundVolume);
        
        // Screen shake
        _cameraShaker.ShakePlayerCamera(bulletData.explosiveData.screenShakeData);
        
        // Big explosion hurt all enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, bulletData.explosiveData.explosiveRadius, bulletData.explosiveData.whatDoesExplosionHit);

        foreach (Collider2D targetCollider in hitEnemies)
        {
            IHealth healthScript = targetCollider.gameObject.GetComponent<IHealth>();

            healthScript?.ModifyHealth(-1f * bulletData.explosiveDamage);
        }
    }
    
    private void PlayParticleEffect()
    {
        GameObject newParticleEffect = ObjectPooler.Instance.GetPooledObject(_explosiveParticlePoolKey);

        _particleEffectScript = newParticleEffect.GetComponent<PoolableParticle>();
        
        _particleEffectScript.PlaceParticleOnTransform(transform);
        
        _particleEffectScript.PlayAllParticles(bulletData.explosiveData.explosiveRadius);
    }
}
