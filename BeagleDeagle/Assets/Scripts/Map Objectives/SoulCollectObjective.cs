using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoulCollectObjective : MapObjective
{
    [SerializeField] private EnemyEvents enemyEvents;
    
    // How many souls need to be collected? 
    [SerializeField, Range(1,100)] 
    private int collectRequirement;
    
    private int _collectedSouls = 0;
    
    // How close do enemies need to be to have their soul get collected?
    [SerializeField] private float collectRange;

    protected override void OnObjectiveEnable()
    {
        base.OnObjectiveEnable();
        enemyEvents.onEnemyDeathGiveGameObject += CollectEnemySoul;
    }
    
    protected override void OnObjectiveDisable()
    {
        base.OnObjectiveDisable();
        
        _collectedSouls = 0;
        
        enemyEvents.onEnemyDeathGiveGameObject -= CollectEnemySoul;
    }

    private void CollectEnemySoul(GameObject enemyThatDied)
    {
        if (Vector2.Distance(transform.position, enemyThatDied.transform.position) <= collectRange)
        {
            _collectedSouls++;
            Debug.Log($"Collected {enemyThatDied}'s soul. Now this has {_collectedSouls} souls."); 
        }

        if (_collectedSouls >= collectRequirement)
        {
            Debug.Log("MAX SOULS HAS BEEN REACHED. OBJECTIVE COMPLETED!");
            
            OnObjectiveCompletion();
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, collectRange);
    }
}
