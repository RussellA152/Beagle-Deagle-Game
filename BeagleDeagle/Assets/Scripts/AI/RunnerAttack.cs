using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerAttack : AIAttack<RunnerEnemyData>
{
    // The collider that will touch this enemy's target
    //private Collider2D _hitBox;

    public BoxCollider2D[] _hitBoxes;
    
    // A list of all colliders that weres damaged by this enemy's attack
    private List<Collider2D> _collidersDamaged = new List<Collider2D>();

    private void Start()
    {
        // Grab all hitboxes inside of the enemy
        _hitBoxes = GetComponentsInChildren<BoxCollider2D>();
        
        //Debug.Log(_hitBox);
    }

    public override void InitiateAttack()
    {
        // For each hitbox on the enemy, check if any of them collided with something
        foreach (BoxCollider2D hitBox in _hitBoxes)
        {
            CheckHitBox(hitBox);
        }

    }

    ///-///////////////////////////////////////////////////////////
    /// Check what the enemy's hitbox collided with, and then try to apply
    /// damage those gameObjects
    /// 
    private void CheckHitBox(Collider2D hitBox)
    {
        Collider2D[] collidersToDamage = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        
        // Only collide with what this enemy can attack
        filter.SetLayerMask(enemyScriptableObject.whatHitBoxDamages);

        int colliderCount = Physics2D.OverlapCollider(hitBox, filter, collidersToDamage);

        // For each enemy this hitBox collided with
        for (int i = 0; i < colliderCount; i++)
        {
            // If this collider was not already damaged by the enemy, then apply damage
            if (!_collidersDamaged.Contains(collidersToDamage[i]))
            {
                _collidersDamaged.Add(collidersToDamage[i]);
                collidersToDamage[i].gameObject.GetComponent<IHealth>().ModifyHealth(-1f * enemyScriptableObject.attackDamage);
            }
                
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// When the enemy's attack animation ends, reset all colliders damaged
    /// 
    public void ResetCollidersDamaged()
    {
        _collidersDamaged.Clear();
    }
    
}
