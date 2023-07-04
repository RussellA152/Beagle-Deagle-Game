using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData/RunnerEnemyWave")]
public class RunnerWaveData : WaveData
{
    public List<RunnerMiniWaveData> miniWaves;
    
    public override List<MiniWaveData> GetMiniWaveData()
    {
        List<MiniWaveData> nMiniWaves = new List<MiniWaveData>();
        
        foreach (RunnerMiniWaveData data in miniWaves)
        {
            nMiniWaves.Add(data);
        }
        
        return nMiniWaves;
    }
}
