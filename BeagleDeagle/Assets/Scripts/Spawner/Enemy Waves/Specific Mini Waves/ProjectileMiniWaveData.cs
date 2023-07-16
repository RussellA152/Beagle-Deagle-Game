using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMiniWave", menuName = "ScriptableObjects/MiniWaveData/ProjectileEnemyMiniWave")]
public class ProjectileMiniWaveData : MiniWaveData
{
    [RestrictedPrefab(typeof(AIBehavior<ProjectileEnemyData>))]
    public GameObject enemyPrefab;
    
    public ProjectileEnemyData enemyData;

    public override GameObject GetEnemyPrefab()
    {
        return enemyPrefab;
    }

    public override EnemyData GetEnemyData()
    {
        return enemyData;
    }
}
