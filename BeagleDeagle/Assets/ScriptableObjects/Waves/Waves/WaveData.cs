using System;
using UnityEngine;
using System.Collections.Generic;


// We can use ScriptableObjects for data about our weapons.
// This is useful for our weapon upgrades since we can specify new damage or ammo values for the upgraded version
[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData/Wave")]
public class WaveData : ScriptableObject
{
    public string name;

    public string message; // this string will be displayed on the screen when this mini wave begins

    [NonReorderable]
    public List<MiniWaveData> miniWaves;

    [HideInInspector]
    public float duration; // the duration of the wave is the maxmimum value in the mini waves
}


