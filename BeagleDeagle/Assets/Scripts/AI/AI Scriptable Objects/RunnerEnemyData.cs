using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRunnerEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/RunnerEnemy")]
public class RunnerEnemyData : EnemyData
{
    [Space(25f)]
    
    // What layer can this enemy's hitBox damage (ex. Player)
    public LayerMask whatHitBoxDamages;
}
