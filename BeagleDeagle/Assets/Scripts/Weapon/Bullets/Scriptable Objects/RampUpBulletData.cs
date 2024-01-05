using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRampUpBullet", menuName = "ScriptableObjects/Projectile/RampUpBullet")]
public class RampUpBulletData : BulletData
{
    [Range(0.05f, 1f)]
    public float damageIncreasePerHit;
    
    public float duration;
    
    public override float GetLifeTime()
    {
        return duration;
    }
}
