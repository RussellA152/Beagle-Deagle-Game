using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBomberEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/BomberEnemy")]
public class BomberEnemyData : EnemyData
{
    [Space(25f)]
    
    public ExplosiveTypeData explosiveType;

    [Range(1f, 30f)] public float explosiveAoeDuration;
}
