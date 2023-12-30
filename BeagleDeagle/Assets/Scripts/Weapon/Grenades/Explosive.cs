using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class Explosive : MonoBehaviour, IExplosiveUpdatable, IPoolable
{
    protected ExplosiveData ExplosiveData;

    [SerializeField] private int poolKey;

    public int PoolKey => poolKey;

    // Should this explosive destroy or disable? (use object pool or not)
    [SerializeField] protected bool shouldDestroy;
    
    [SerializeField] protected GameObject sprite;
    
    [SerializeField, RestrictedPrefab(typeof(PoolableParticle))] 
    private GameObject explosiveParticleGameObject;

    private PoolableParticle _particleEffectScript;

    private int _explosiveParticlePoolKey;

    protected AreaOfEffect AreaOfEffectScript;

    protected CheckObstruction ObstructionScript;

    private CameraShaker _cameraShaker;

    private AudioClipPlayer _audioClipPlayer;
    
    protected int WallLayerMask;

    protected float Damage;

    protected float Duration;
    
    // Do something to an enemy that gets hit by this explosion
    [SerializeField] private UnityEvent<GameObject> onExplosionHitTarget;

    protected virtual void Awake()
    {
        ObstructionScript = GetComponentInParent<CheckObstruction>();
        AreaOfEffectScript =
            GetComponentInChildren<AreaOfEffect>();

        _cameraShaker = GetComponent<CameraShaker>();
        _audioClipPlayer = GetComponent<AudioClipPlayer>();
        
        // Find explosive effect if this gameObject needs one
        if(explosiveParticleGameObject != null)
            _explosiveParticlePoolKey = explosiveParticleGameObject.GetComponent<IPoolable>().PoolKey;

    }

    private void Start()
    {
        // Disable AOE at start
        if(AreaOfEffectScript != null)
            AreaOfEffectScript.gameObject.SetActive(false);
        
        WallLayerMask = LayerMask.GetMask("Wall");
    }

    protected virtual void OnDisable()
    {
        if(AreaOfEffectScript != null)
            AreaOfEffectScript.gameObject.SetActive(false);

        sprite.SetActive(true);

        StopAllCoroutines();
    }

    public virtual void Activate(Vector2 aimDirection)
    {
        if(AreaOfEffectScript != null)
            AreaOfEffectScript.UpdateAOEData(ExplosiveData.aoeData);
    }

    
    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public IEnumerator Detonate()
    {
        yield return new WaitForSeconds(ExplosiveData.detonationTime);

        AfterDetonation();

        yield return new WaitForSeconds(Duration);

        AfterDurationEnds();
    }

    protected virtual void AfterDetonation()
    {
        if (AreaOfEffectScript != null)
        {
            AreaOfEffectScript.gameObject.SetActive(true);
            
            // Tell AOE to activate itself
            AreaOfEffectScript.OnAreaOfEffectActivate();
            
            // Play AOE sound for "Duration" amount of seconds
            // We play this here because AOE depend on explosives to activate
            AreaOfEffectScript.PlayAreaofEffectSound(Duration);
        }
        
        Explode();
        
        if(explosiveParticleGameObject != null) 
            PlayParticleEffect();
    }

    protected virtual void AfterDurationEnds()
    {
        // We destroy the nuke instead of disabling it because we don't pool nukes at the moment
        if(shouldDestroy)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    protected virtual void Explode()
    {
        // Play explosion sound
        PlayExplosionSound();
        
        // Screen shake
        _cameraShaker.ShakePlayerCamera(ExplosiveData.screenShakeData);
        
        // Big explosion hurt all enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, ExplosiveData.explosiveRadius, ExplosiveData.whatDoesExplosionHit);

        foreach (Collider2D targetCollider in hitEnemies)
        {
            if (!ObstructionScript.HasObstruction(transform.position, targetCollider.gameObject, WallLayerMask))
            {
                IHealth healthScript = targetCollider.gameObject.GetComponent<IHealth>();

                healthScript?.ModifyHealth(-1f * Damage);
                
                onExplosionHitTarget?.Invoke(targetCollider.gameObject);
            }
        }
    }

    private void PlayExplosionSound()
    {
        _audioClipPlayer.PlayRandomGeneralAudioClip(ExplosiveData.explosionClips, ExplosiveData.explosiveSoundVolume);
        
    }
    
    private void PlayParticleEffect()
    {
        GameObject newParticleEffect = ObjectPooler.Instance.GetPooledObject(_explosiveParticlePoolKey);

        _particleEffectScript = newParticleEffect.GetComponent<PoolableParticle>();
        
        _particleEffectScript.PlaceParticleOnTransform(transform);
        
        _particleEffectScript.PlayAllParticles(ExplosiveData.explosiveRadius);
    }
    

    public void SetDamage(float explosiveDamage)
    {
        Damage = explosiveDamage;
    }

    public void SetDuration(float explosiveDuration)
    {
        Duration = explosiveDuration;
    }

    public float GetDetonationTime()
    {
        return ExplosiveData.detonationTime;
    }
    public virtual void UpdateScriptableObject(ExplosiveData scriptableObject)
    {
        ExplosiveData = scriptableObject;
    }
}
