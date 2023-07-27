using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/ProjectileEnemy")]
public class ProjectileEnemyData : EnemyData
{
    [Header("Projectile Logic")]
    public BulletTypeData bulletType;

    [Range(1, 50)]
    public int bulletPenetration;
    
    // How many projectiles does this enemy shoot at a time?
    [Range(1, 20)]
    public int numberOfProjectiles;
}
