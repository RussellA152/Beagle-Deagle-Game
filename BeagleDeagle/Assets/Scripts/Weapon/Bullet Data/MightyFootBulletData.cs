using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/MightyFootBullet")]
public class MightyFootBulletData : BulletData
{
    [Header("Utility Ability That Activates This")]
    [SerializeField]
    private UtilityAbilityData utilityAbilityData;

    [Range(0f, 150f)]
    public int numEnemiesCanHit;

    [Header("Stun Effect")]
    [Range(0f, 30f)]
    public float stunDuration; // How long will the enemy be stunned when hit by this?

    [FormerlySerializedAs("knockbackForce")] [Header("Knockback Power")]
    public Vector2 knockBackForce;

    public override void OnHit(Rigidbody2D bulletRb, GameObject objectHit, float damage)
    {
        // Make target take damage
        base.OnHit(bulletRb, objectHit, damage);
        
        // Stun the enemy for a certain amount of seconds
        objectHit.GetComponent<IStunnable>().GetStunned(stunDuration);

        Vector2 knockBackDirection = bulletRb.velocity.normalized;
        objectHit.GetComponent<IKnockBackable>().ApplyKnockBack(knockBackDirection, knockBackForce);

    }

    public override float GetLifeTime()
    {
        return utilityAbilityData.duration;
    }

}
