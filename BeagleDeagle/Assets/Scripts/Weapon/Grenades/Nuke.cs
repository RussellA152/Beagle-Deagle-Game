using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Nuke : Explosive<NukeData>, IPoolable
{
    [SerializeField] private int poolKey;

    [SerializeField, RestrictedPrefab(typeof(PoolableParticle))] 
    private GameObject explosiveParticleGameObject;

    private PoolableParticle _particleEffectScript;

    private int _explosiveParticlePoolKey;

    public int PoolKey => poolKey;
    
    private bool _explosionHappening = false;

    protected override void Awake()
    {
        base.Awake();

        _explosiveParticlePoolKey = explosiveParticleGameObject.GetComponent<IPoolable>().PoolKey;

    }
    
    public override void Activate(Vector2 aimDirection)
    {
        base.Activate(aimDirection);
        
        transform.position = aimDirection;
        
        StartCoroutine(Detonate());
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public override IEnumerator Detonate()
    {
        yield return new WaitForSeconds(ExplosiveData.detonationTime);

        sprite.SetActive(false);

        if (AreaOfEffectScript != null)
        {
            AreaOfEffectScript.gameObject.SetActive(true);
            AreaOfEffectScript.OnAreaOfEffectActivate();
        }
            

        StartCoroutine(BrieflyShowGizmo());

        Explode();

        yield return new WaitForSeconds(Duration);

        // We destroy the nuke instead of disabling it because we don't pool nukes at the moment
        if(shouldDestroy)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);

    }

    protected override void Explode()
    {
        base.Explode();

        PlayParticleEffect();
        
        // Big explosion hurt all enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, ExplosiveData.explosiveRadius, ExplosiveData.whatDoesExplosionHit);

        foreach (Collider2D targetCollider in hitEnemies)
        {
            if (!ObstructionScript.HasObstruction(transform.position, targetCollider.gameObject, WallLayerMask))
            {
                IHealth healthScript = targetCollider.gameObject.GetComponent<IHealth>();

                healthScript?.ModifyHealth(-1f * Damage);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (_explosionHappening)
        {
            Gizmos.color = Color.red;
        
            Gizmos.DrawWireSphere(transform.position, ExplosiveData.explosiveRadius);
        }
    }

    private IEnumerator BrieflyShowGizmo()
    {
        _explosionHappening = true;
        yield return new WaitForSeconds(0.5f);
        _explosionHappening = false;
    }

    private void PlayParticleEffect()
    {
        GameObject newParticleEffect = ObjectPooler.Instance.GetPooledObject(_explosiveParticlePoolKey);

        _particleEffectScript = newParticleEffect.GetComponent<PoolableParticle>();
        
        _particleEffectScript.PlaceParticleOnTransform(transform);
        
        _particleEffectScript.PlayAllParticles(ExplosiveData.explosiveRadius);
    }
    
}
