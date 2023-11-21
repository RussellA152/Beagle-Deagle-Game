using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// A series of significant events caused by an enemy. For example, when an enemy dies they should give a certain amount of xp, which listeners
/// retrieve.
/// 
[CreateAssetMenu(menuName = "GameEvent/EnemyEvents")]
public class EnemyEvents : ScriptableObject
{
    // Pass the gameObject of the enemy that died
    public event Action<GameObject> onEnemyDeathGiveGameObject;

    ///-///////////////////////////////////////////////////////////
    /// When an enemy is killed, pass their gameObject to other scripts
    /// that need a reference.
    /// 
    public void InvokeEnemyDeathGiveGameObject(GameObject enemyGameObject)
    {
        onEnemyDeathGiveGameObject?.Invoke(enemyGameObject);
    }
}
