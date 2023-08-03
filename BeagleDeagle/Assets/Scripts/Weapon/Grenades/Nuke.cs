using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Explosive<NukeData>, IPoolable
{
    [SerializeField] private int poolKey;
    public int PoolKey => poolKey;
    
    private bool _explosionHappening = false;
    

    public override void Activate(Vector2 aimDirection)
    {
        transform.position = aimDirection;
        StartCoroutine(Detonate());
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public override IEnumerator Detonate()
    {
        yield return new WaitForSeconds(explosiveData.detonationTime);

        sprite.SetActive(false);
        
        if(areaOfEffectGameObject != null)
            areaOfEffectGameObject.gameObject.SetActive(true);

        StartCoroutine(BrieflyShowGizmo());

        Explode();

        yield return new WaitForSeconds(Duration);

        // We destroy the nuke instead of disabling it because we don't pool nukes at the moment
        gameObject.SetActive(false);

    }

    public override void Explode()
    {
        base.Explode();
        
        // Big explosion hurt all enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosiveData.explosiveRadius, explosiveData.whatDoesExplosionHit);

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

            Gizmos.DrawWireSphere(transform.position, explosiveData.explosiveRadius);
        }
        
    }

    private IEnumerator BrieflyShowGizmo()
    {
        _explosionHappening = true;
        yield return new WaitForSeconds(0.5f);
        _explosionHappening = false;
    }
}
