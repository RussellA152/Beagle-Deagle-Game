using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMiniWave", menuName = "ScriptableObjects/MiniWaveData/RunnerMiniWave")]
public class RunnerMiniWaveData : MiniWaveData
{
    [RestrictedPrefab(typeof(AIBehavior<RunnerEnemyData>))]
    public GameObject enemyPrefab;
    
    public RunnerEnemyData enemyData;

    public override GameObject GetEnemyPrefab()
    {
        return enemyPrefab;
    }

    public override EnemyData GetEnemyData()
    {
        return enemyData;
    }
}
