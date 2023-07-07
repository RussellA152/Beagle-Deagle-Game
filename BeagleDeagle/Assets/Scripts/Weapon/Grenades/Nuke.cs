using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Explosive<NukeData>
{
    private bool _explosionHappening = false;

    private void OnEnable()
    {
        ActivateNuclearBomb();
    }

    private void OnDisable()
    {
        areaOfEffectGameObject.gameObject.SetActive(false);
        sprite.SetActive(true);

        StopAllCoroutines();
    }

    private void ActivateNuclearBomb()
    {
        StartCoroutine(Detonate());
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public override IEnumerator Detonate()
    {
        yield return new WaitForSeconds(explosiveData.detonationTime);

        sprite.SetActive(false);
        areaOfEffectGameObject.gameObject.SetActive(true);

        StartCoroutine(BrieflyShowGizmo());

        Explode();

        yield return new WaitForSeconds(explosiveData.GetDuration());

        // We destroy the nuke instead of disabling it because we don't pool nukes at the moment
        Destroy(gameObject);

    }

    protected override void Explode()
    {
        base.Explode();
        
        // Big explosion hurt all enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosiveData.explosiveRadius, explosiveData.whatDoesExplosionHit);

        foreach (Collider2D targetCollider in hitEnemies)
        {
            if(!CheckObstruction(targetCollider))
                targetCollider.gameObject.GetComponent<IHealth>().ModifyHealth(-1f * explosiveData.GetDamage());

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
