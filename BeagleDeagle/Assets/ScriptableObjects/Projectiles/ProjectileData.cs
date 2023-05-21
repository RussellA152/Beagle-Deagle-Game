using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileData : ScriptableObject
{
    public float bulletSpeed = 15f;

    [Range(0f, 30f)]
    public float destroyTime = 3f;

    public virtual void ApplyTrajectory(Rigidbody2D rb, Transform transform)
    {
        rb.velocity = transform.right * bulletSpeed;

    }


}
