using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackOnCollision : StatusEffect<KnockBackData>
{
    [SerializeField] 
    // The rigidbody of this gameObject
    private Rigidbody2D rb;
    public override void ApplyEffect(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            //Vector2 knockBackDirection = rb.velocity.normalized;
            Vector2 knockBackDirection = rb.velocity.normalized;
        
            objectHit.GetComponent<IKnockBackable>().ApplyKnockBack(knockBackDirection, statusEffectData.knockBackPower);
            
        }
        
    }
}
