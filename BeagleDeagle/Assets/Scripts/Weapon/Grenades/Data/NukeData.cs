using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosive/Nuclear Bomb")]
public class NukeData : ExplosiveData
{
    [SerializeField]
    private UltimateAbilityData ultimateAbilityData;
    
    public float explosiveRadius;

    public LayerMask whatDoesExplosionHit;


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
