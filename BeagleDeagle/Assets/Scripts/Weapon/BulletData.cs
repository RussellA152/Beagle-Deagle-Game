using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile/RegularBullet")]
public abstract class BulletData : ScriptableObject
{
    public Sprite bulletSprite;

    //[Header("Damage (*Added On Top of Gun's Damage*)")]
    //[Range(0f, 500f)]
    //public float bulletDamage; // How much damage does this bullet do? (added on top of gun's damage)

    public LayerMask whatBulletCanPenetrate; // What will NOT cause this bullet to destroy on collision (Ex. Bullet gets destroyed when hitting a wall)
    public LayerMask whatBulletCanDamage; // What will this bullet attempt to damage? (Ex. Player shoots a bullet that damages enemies)

    //[Header("Penetration (*Added On Top of Gun's Penetration*)")]
    //[Range(0f, 100f)]
    //public int bulletPenetration; // How many enemies can this bullet penetrate through? (added on top of gun's penetration)

    [Header("Speed of Bullet")]
    public float bulletSpeed = 15f;

    [Header("Size of Bullet")]
    public CapsuleDirection2D colliderDirection; // Will this bullet's collider shape be horizontal or vertical?
    [Range(0f, 100f)]
    public float sizeX; // What is the width of this bullet's collider?
    [Range(0f, 100f)]
    public float sizeY; // What is the height of this bullet's collider?

    //[Header("How Long Will Bullet Stay Active")]
    //[Range(0f, 30f)]
    //public float destroyTime = 3f;

    // The trajectory of the bullet can differ with each bullet (Ex. regular vs. homing)
    public virtual void ApplyTrajectory(Rigidbody2D rb, Transform transform)
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    // What does this bullet do when it hits an enemy?
    // By default, it will apply some damage on the enemy 
    public virtual void OnHit(Rigidbody2D bulletRb, GameObject objectHit, float damage)
    {
        // Make target take damage
        objectHit.GetComponent<IHealth>().ModifyHealth(-1 * damage);
    }

    public abstract float GetLifeTime();

}
