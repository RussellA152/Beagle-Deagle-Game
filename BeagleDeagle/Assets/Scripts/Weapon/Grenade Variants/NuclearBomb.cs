using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosive/Nuclear Bomb")]
public class NuclearBomb : GrenadeData
{
    [SerializeField]
    private UltimateAbilityData ultimateAbilityData;

    [Range(0f, 40f)]
    public float explosiveRadius;

    public override void Explode(Vector2 explosionSource)
    {
        // Big explosion hurt all enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(explosionSource, explosiveRadius, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D targetCollider in hitEnemies)
        {
            if(!CheckObstruction(explosionSource, targetCollider))
                targetCollider.gameObject.GetComponent<IHealth>().ModifyHealth(-1f * ultimateAbilityData.abilityDamage);

        }
    }

    public override float GetDuration()
    {
        // ultimate ability duration
        return ultimateAbilityData.duration;
    }

}
