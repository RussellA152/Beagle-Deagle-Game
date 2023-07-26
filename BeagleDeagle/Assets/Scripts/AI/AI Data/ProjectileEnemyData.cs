using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/ProjectileEnemy")]
public class ProjectileEnemyData : EnemyData
{
    public GameObject projectilePrefab;
    
    // How many projectiles does this enemy shoot at a time?
    [Range(1, 20)]
    public int numberOfShots;
}
