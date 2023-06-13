using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon/Gun")]
public class RegularGunData : GunData
{
    [Header("Damage")]
    [Range(0, 1000f)]
    public float damagePerHit;

    public override float GetDamage()
    {
        return damagePerHit;
    }
}
