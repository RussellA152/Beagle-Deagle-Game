using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCollectObjective : MapObjective
{
    [SerializeField] private EnemyEvents enemyEvents;
    
    private int _collectedSouls = 0;
    [SerializeField] private float collectRange;

    private void Start()
    {
        //base.Start();
        enemyEvents.onEnemyDeathGiveGameObject += CollectEnemySoul;
    }

    private void CollectEnemySoul(GameObject enemyThatDied)
    {
        if (Vector2.Distance(transform.position, enemyThatDied.transform.position) <= collectRange)
        {
            _collectedSouls++;
            Debug.Log($"Collected {enemyThatDied}'s soul. Now this has {_collectedSouls} souls."); 
        }
        
    }

    protected override void OnObjectiveDisable()
    {
        _collectedSouls = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, collectRange);
    }
}
