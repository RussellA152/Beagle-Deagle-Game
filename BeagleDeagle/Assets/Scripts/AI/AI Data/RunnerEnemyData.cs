using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRunnerEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/RunnerEnemy")]
public class RunnerEnemyData : EnemyData
{
    public LayerMask whatHitBoxDamages;
}
