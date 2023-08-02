using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBomberEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/BomberEnemy")]
public class BomberEnemyData : EnemyData
{
    [Space(25f)]
    
    public ExplosiveTypeData explosiveType;

    // How long will the explosive gameObject remain (meant for explosives with AOEs)
    [Range(0.1f, 30f)] public float explosiveDuration;
}
