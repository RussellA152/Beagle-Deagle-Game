using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosive/Smoke Grenade")]
public class SmokeGrenadeData : GrenadeData
{
    [FormerlySerializedAs("utilityAbilityData")] [SerializeField]
    private SmokeGrenadeUtilityData utilityAbilityData;

    public override float GetDuration()
    {
        return utilityAbilityData.duration;
    }
}
