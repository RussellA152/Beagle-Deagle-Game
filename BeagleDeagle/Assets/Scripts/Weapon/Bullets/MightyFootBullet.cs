using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightyFootBullet : Bullet<MightyFootBulletData>
{
    private int _wallLayerMask;

     private void Start()
     {
         _wallLayerMask = LayerMask.GetMask("Wall");
     }
    
    protected override void OnHit(GameObject objectHit)
    {
        if (CheckObstruction(objectHit.GetComponent<Collider2D>())) 
            return;
        
        // Make target take damage
        base.OnHit(objectHit);
        
        // Stun the enemy for a certain amount of seconds
        objectHit.GetComponent<IStunnable>().GetStunned(bulletData.stunDuration);

        Vector2 knockBackDirection = rb.velocity.normalized;
        objectHit.GetComponent<IKnockBackable>().ApplyKnockBack(knockBackDirection, bulletData.knockBackForce);

    }

    private bool CheckObstruction(Collider2D targetCollider)
    {
        // TEMPORARY, JUST TESTING
        Vector2 startingPoint = GameObject.Find("Deagle Beagle").transform.position;

        Vector3 targetPosition = targetCollider.transform.position;

        Vector3 direction = targetPosition - transform.position;

        float distance = direction.magnitude;


        // Exclude the target if there is an obstruction between the explosion source and the target
        RaycastHit2D hit = Physics2D.Raycast(startingPoint, direction.normalized, distance, _wallLayerMask);

        if (hit.collider != null)
        {
            Debug.Log("OBSTRUCTION FOUND!");
            // There is an obstruction, so skip this target
            return true;
        }
        else
        {
            // If no obstruction found, allow this target to be affected by the explosion
            return false;
        }
    }

}
