using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBulletData : BulletData
{
    [Header("Ability That Activates This")]
    [SerializeField]
    private AbilityData abilityData;

    public override float GetLifeTime()
    {
        return abilityData.duration;
    }
}
