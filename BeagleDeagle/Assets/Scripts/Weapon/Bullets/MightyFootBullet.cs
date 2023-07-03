using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightyFootBullet : Bullet<MightyFootBulletData>
{
    // Who shot this bullet? (usually player)
    private Transform _whoShotThisBullet;
    
    private int _wallLayerMask;

     private void Start()
     {
         _wallLayerMask = LayerMask.GetMask("Wall");
     }
    
    protected override void OnHit(GameObject objectHit)
    {
        if (CheckObstruction(objectHit)) 
            return;
        
        // Make target take damage
        base.OnHit(objectHit);
        
        // Stun the enemy for a certain amount of seconds
        objectHit.GetComponent<IStunnable>().GetStunned(bulletData.stunDuration);

        Vector2 knockBackDirection = rb.velocity.normalized;
        objectHit.GetComponent<IKnockBackable>().ApplyKnockBack(knockBackDirection, bulletData.knockBackForce);

    }

    public void UpdateWhoShotThisBullet(Transform caster)
    {
        _whoShotThisBullet = caster;
    }

    private bool CheckObstruction(GameObject objectHit)
    {
        // Start from whoever shot this MightyFootBullet (usually the player)
        Vector2 startingPoint = _whoShotThisBullet.position;

        // Will shoot a raycast at whoever the bullet hit
        Vector3 targetPosition = objectHit.transform.position;

        Vector3 direction = targetPosition - transform.position;

        float distance = direction.magnitude;


        // Exclude the target if there is an obstruction between whoever shot the bullet,  and the target
        RaycastHit2D hit = Physics2D.Raycast(startingPoint, direction.normalized, distance, _wallLayerMask);

        if (hit.collider != null)
        {
            Debug.Log("OBSTRUCTION FOUND!");
            // There is an obstruction, so skip this target
            return true;
        }
        else
        {
            // If no obstruction found, allow this target to be affected by the bullet
            return false;
        }
    }
}
