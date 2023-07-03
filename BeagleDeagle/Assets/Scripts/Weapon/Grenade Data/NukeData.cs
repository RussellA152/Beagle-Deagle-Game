using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosive/Nuclear Bomb")]
public class NukeData : GrenadeData
{
    [SerializeField]
    private UltimateAbilityData ultimateAbilityData;
    
    //public RadiationAreaOfEffectData radiationAOEData;
    
    public float explosiveRadius;

    public LayerMask whatDoesExplosionHit;
    // public override void Explode(Vector2 explosionSource)
    // {
    //     // Big explosion hurt all enemies
    //     Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(explosionSource, explosiveRadius, whatDoesExplosionHit);
    //
    //     foreach (Collider2D targetCollider in hitEnemies)
    //     {
    //         if(!CheckObstruction(explosionSource, targetCollider))
    //             targetCollider.gameObject.GetComponent<IHealth>().ModifyHealth(-1f * ultimateAbilityData.abilityDamage);
    //
    //     }
    // }

    public override float GetDamage()
    {
        return ultimateAbilityData.abilityDamage;
    }

    public override float GetDuration()
    {
        // ultimate ability duration
        return ultimateAbilityData.duration;
    }

}
