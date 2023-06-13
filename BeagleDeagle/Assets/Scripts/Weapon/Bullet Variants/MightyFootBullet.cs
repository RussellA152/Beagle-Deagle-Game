using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/MightyFootBullet")]
public class MightyFootBullet : AbilityBulletData
{
    [Range(0f, 150f)]
    public int numEnemiesCanHit;

    [Header("Stun Effect")]
    [Range(0f, 30f)]
    public float stunDuration; // How long will the enemy be stunned when hit by this?

    [Header("Knockback Power")]
    public Vector2 knockbackForce;

    public override void OnHit(Rigidbody2D bulletRb, GameObject objectHit, float damage)
    {
        // Make target take damage
        base.OnHit(bulletRb, objectHit, damage);
        
        // Stun the enemy for a certain amount of seconds
        objectHit.GetComponent<IStunnable>().GetStunned(stunDuration);

        Vector2 knockbackDirection = bulletRb.velocity.normalized;
        objectHit.GetComponent<IKnockBackable>().ApplyKnockback(knockbackDirection, knockbackForce);

    }

}
