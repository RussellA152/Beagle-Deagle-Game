using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class BulletData : ScriptableObject
{
    public Sprite bulletSprite;

    [Header("Collision Behavior")]
    public LayerMask whatBulletCanPenetrate; // What will this bullet penetrate through (Ex. Bullet penetrates through enemies, or walls to hit enemies as well)
    public LayerMask whatBulletCanDamage; // What will this bullet attempt to damage? (Ex. Player shoots a bullet that damages enemies)
    public LayerMask whatDestroysBullet; // What will destroy this bullet if it touches (Ex. Bullet touches wall and get destroyed)

    [Header("Speed of Bullet")]
    public float bulletSpeed = 15f;

    [Header("Size of Bullet")]
    public CapsuleDirection2D colliderDirection; // Will this bullet's collider shape be horizontal or vertical?
    [Range(0f, 100f)]
    public float sizeX; // What is the width of this bullet's collider?
    [Range(0f, 100f)]
    public float sizeY; // What is the height of this bullet's collider?
    

    ///-///////////////////////////////////////////////////////////
    /// Return the "duration" of the bullet
    /// Might come from the bullet itself, or an ability
    ///
    public abstract float GetLifeTime();

}
