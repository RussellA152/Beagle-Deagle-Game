using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/RegularBullet")]
public class RegularBulletData : BulletData
{
    [Header("Lifetime For This Grenade")]
    [SerializeField]
    private float duration;

    public override float GetLifeTime()
    {
        return duration;
    }
}
