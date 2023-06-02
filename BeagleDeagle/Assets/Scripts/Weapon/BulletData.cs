using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/RegularBullet")]
public class BulletData : ScriptableObject
{
    public LayerMask whatBulletCanPenetrate; // What will cause this bullet to destroy on collision (Ex. Bullet gets destroyed when hitting a wall)
    public LayerMask whatBulletCanDamage; // What will this bullet attempt to damage? (Ex. Player shoots a bullet that damages enemies)

    public float bulletSpeed = 15f;

    [Range(0f, 30f)]
    public float destroyTime = 3f;

    // The trajectory of the bullet can differ with each bullet (Ex. regular vs. homing)
    public virtual void ApplyTrajectory(Rigidbody2D rb, Transform transform)
    {
        rb.velocity = transform.right * bulletSpeed;

    }

}
