using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Explosive<NuclearBomb>
{
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

        explosiveData.Explode(transform.position);

        yield return new WaitForSeconds(explosiveData.GetDuration());

        gameObject.SetActive(false);

    }

    public override void UpdateExplosiveData(NuclearBomb scriptableObject)
    {
        base.UpdateExplosiveData(scriptableObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, explosiveData.explosiveRadius);
    }

}
