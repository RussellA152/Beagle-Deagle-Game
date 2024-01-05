using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRicochetBullet", menuName = "ScriptableObjects/Projectile/RicochetBullet")]
public class RicochetBulletData : BulletData
{
    [Header("Lifetime For This Bullet")] 
    [Range(2, 10)] public int ricochetLimit; // Number of times this bullet can reflect off the screen before it disables
    [SerializeField] private float duration;

    public override float GetLifeTime()
    {
        return duration;
    }
}
