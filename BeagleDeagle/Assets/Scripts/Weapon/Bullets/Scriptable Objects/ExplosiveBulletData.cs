using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosiveBullet", menuName = "ScriptableObjects/Projectile/ExplosiveBullet")]
public class ExplosiveBulletData : BulletData
{
    // Explosive stats for the bullet's explosion
    public ExplosiveData explosiveData;
    
    [Range(0, 1000f)]
    public float explosiveDamage;
    
    [Header("Lifetime For This Bullet")]
    [SerializeField] private float duration;

    public override float GetLifeTime()
    {
        return duration;
    }
}
