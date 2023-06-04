using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/MightyFootBullet")]
public class MightyFootBullet : BulletData
{
    [Range(0f, 500f)]
    public float damagePerHit; // How much damage does this bullet do?

    [Range(0f, 100f)]
    public int numEnemiesCanHit; // How many enemies can this bullet hit?

    [Range(0f, 30f)]
    public float stunDuration; // How long will the enemy be stunned when hit by this?

    public override void ApplyTrajectory(Rigidbody2D rb, Transform transform)
    {
        rb.velocity = transform.right * bulletSpeed;

    }

    public override void OnHit(Collider2D collision, float damage)
    {
        // Make target take damage
        collision.gameObject.GetComponent<IHealth>().ModifyHealth(-1 * damage);

        collision.gameObject.GetComponent<IStunnable>().GetStunned(stunDuration);

        // TESTING ADD FORCE
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(250f,100f));
    }


}
