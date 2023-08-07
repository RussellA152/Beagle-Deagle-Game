using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/ProjectileEnemy")]
public class ProjectileEnemyData : EnemyData
{
    [Space(25f)]
    
    [Header("Projectile Logic")]
    // What kind of bullet will this enemy shoot? Ex. regular, fire
    public BulletTypeData bulletType;

    // How much penetration does this enemy's bullet have?
    [Range(1, 50)]
    public int bulletPenetration;
    
    // How many projectiles does this enemy shoot at a time?
    [Range(1, 20)]
    public int numberOfProjectiles;
}
