using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRunnerEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/RunnerEnemy")]
public class RunnerEnemyData : EnemyData
{
    [Space(25f)]
    
    public LayerMask whatHitBoxDamages;
}
