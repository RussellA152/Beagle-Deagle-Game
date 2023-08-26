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
    // Pass the amount of xp that an enemy will grant upon death
    public event Action<int> onEnemyDeathXp;

    ///-///////////////////////////////////////////////////////////
    /// When an enemy is killed, pass the amount of xp that they should give.
    /// 
    public void InvokeGiveXp(int xpAmount)
    {
        onEnemyDeathXp?.Invoke(xpAmount);
    }
}
