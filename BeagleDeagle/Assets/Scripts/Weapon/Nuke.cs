using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Explosive<NukeData>
{
    private bool explosionHappening = false;

    private void OnEnable()
    {
        areaOfEffect.UpdateAOEData(explosiveData.aoeData);

        ActivateNuclearBomb();
    }

    private void OnDisable()
    {
        areaOfEffect.gameObject.SetActive(false);
        sprite.SetActive(true);

        StopAllCoroutines();
    }

    public void ActivateNuclearBomb()
    {
        StartCoroutine(Detonate());
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public override IEnumerator Detonate()
    {
        yield return new WaitForSeconds(explosiveData.detonationTime);

        sprite.SetActive(false);
        areaOfEffect.gameObject.SetActive(true);

        StartCoroutine(BrieflyShowGizmo());

        explosiveData.Explode(transform.position);

        yield return new WaitForSeconds(explosiveData.GetDuration());

        // We destroy the nuke instead of disabling it because we don't pool nukes at the moment
        Destroy(gameObject);

    }

    public override void UpdateExplosiveData(NukeData scriptableObject)
    {
        base.UpdateExplosiveData(scriptableObject);
    }

    private void OnDrawGizmos()
    {
        if (explosionHappening)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, explosiveData.explosiveRadius);
        }
        
    }

    private IEnumerator BrieflyShowGizmo()
    {
        explosionHappening = true;
        yield return new WaitForSeconds(0.5f);
        explosionHappening = false;
    }

}
