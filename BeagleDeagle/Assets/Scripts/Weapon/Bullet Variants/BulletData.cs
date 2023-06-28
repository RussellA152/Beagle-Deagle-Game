using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/RegularBullet")]
public abstract class BulletData : ScriptableObject
{
    public Sprite bulletSprite;

    public LayerMask whatBulletCanPenetrate; // What will NOT cause this bullet to destroy on collision (Ex. Bullet gets destroyed when hitting a wall)
    public LayerMask whatBulletCanDamage; // What will this bullet attempt to damage? (Ex. Player shoots a bullet that damages enemies)

    [Header("Speed of Bullet")]
    public float bulletSpeed = 15f;

    [Header("Size of Bullet")]
    public CapsuleDirection2D colliderDirection; // Will this bullet's collider shape be horizontal or vertical?
    [Range(0f, 100f)]
    public float sizeX; // What is the width of this bullet's collider?
    [Range(0f, 100f)]
    public float sizeY; // What is the height of this bullet's collider?
    
    ///-///////////////////////////////////////////////////////////
    /// The trajectory of the bullet can differ with each bullet (Ex. regular vs. homing)
    ///
    public virtual void ApplyTrajectory(Rigidbody2D rb, Transform transform)
    {
        rb.velocity = transform.right * bulletSpeed;
    }
    
    ///-///////////////////////////////////////////////////////////
    /// What does this bullet do when it hits an enemy?
    /// By default, it will apply some damage on the enemy
    ///
    public virtual void OnHit(Rigidbody2D bulletRb, GameObject objectHit, float damage)
    {
        // Make target take damage
        objectHit.GetComponent<IHealth>().ModifyHealth(-1 * damage);
    }

    ///-///////////////////////////////////////////////////////////
    /// Return the "duration" of the bullet
    /// Might come from the bullet itself, or an ability
    ///
    public abstract float GetLifeTime();

}
