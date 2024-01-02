using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRunnerEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/RunnerEnemy")]
public class RunnerEnemyData : EnemyData
{
    [Space(25f)]
    
    [Header("Melee Attack Logic")]
    // What layer can this enemy's hitBox damage (ex. Player)
    public LayerMask whatHitBoxDamages;

    [Header("Attack Landing Effects")]
    // Damage sound effect
    public AudioClip[] attackLandedSounds;
    [Range(0.1f, 1f)] public float volume;

    // Screen shake power when attack lands
    public ScreenShakeData screenShakeData;
}
