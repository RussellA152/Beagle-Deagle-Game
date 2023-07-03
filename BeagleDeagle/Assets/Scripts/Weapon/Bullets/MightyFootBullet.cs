using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightyFootBullet : Bullet<MightyFootBulletData>
{
    protected override void OnHit(GameObject objectHit)
    {
        // Make target take damage
        base.OnHit(objectHit);
        
        // Stun the enemy for a certain amount of seconds
        objectHit.GetComponent<IStunnable>().GetStunned(bulletData.stunDuration);

        Vector2 knockBackDirection = rb.velocity.normalized;
        objectHit.GetComponent<IKnockBackable>().ApplyKnockBack(knockBackDirection, bulletData.knockBackForce);
    }
}
