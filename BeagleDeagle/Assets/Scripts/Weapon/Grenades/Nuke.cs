using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Nuke : Explosive<NukeData>, IPoolable
{
    [SerializeField] private int poolKey;
    
    [SerializeField] private GameObject[] explosiveParticleGameObjects;
    
    public int PoolKey => poolKey;
    
    private bool _explosionHappening = false;

    protected override void Awake()
    {
        base.Awake();

        foreach (GameObject explosiveParticle in explosiveParticleGameObjects)
        {
            explosiveParticle.SetActive(false);
        }
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        foreach (GameObject explosiveParticle in explosiveParticleGameObjects)
        {
            explosiveParticle.SetActive(false);
        }
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
        
        if(AreaOfEffectScript != null)
            AreaOfEffectScript.gameObject.SetActive(true);

        StartCoroutine(BrieflyShowGizmo());

        Explode();

        yield return new WaitForSeconds(Duration);

        // We destroy the nuke instead of disabling it because we don't pool nukes at the moment
        gameObject.SetActive(false);

    }

    public override void Explode()
    {
        base.Explode();
        
        foreach (GameObject explosiveParticle in explosiveParticleGameObjects)
        {
            explosiveParticle.SetActive(true);
        }
        
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

    public override void UpdateScriptableObject(ExplosiveData scriptableObject)
    {
        base.UpdateScriptableObject(scriptableObject);
        
        foreach (GameObject explosiveParticle in explosiveParticleGameObjects)
        {
            var localScale = explosiveParticle.transform.localScale;
            
            localScale = new Vector3(ExplosiveData.explosiveRadius *localScale.x ,
                ExplosiveData.explosiveRadius * localScale.y,ExplosiveData.explosiveRadius * localScale.z);
            explosiveParticle.transform.localScale = localScale;
        }
    }
}
