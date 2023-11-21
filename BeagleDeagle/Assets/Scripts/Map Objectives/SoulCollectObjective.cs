using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoulCollectObjective : MapObjective
{
    // How many souls need to be collected? 
    [SerializeField, Range(1,100)] 
    private int collectRequirement;
    
    private int _collectedSouls = 0;
    
    // How close do enemies need to be to have their soul get collected?
    [SerializeField] private float collectRange;

    protected override void OnObjectiveEnable()
    {
        base.OnObjectiveEnable();
        EnemyManager.Instance.onEnemyDeathGiveGameObject += CollectEnemySoul;
    }
    
    protected override void OnObjectiveDisable()
    {
        base.OnObjectiveDisable();
        
        _collectedSouls = 0;
        
        EnemyManager.Instance.onEnemyDeathGiveGameObject -= CollectEnemySoul;
    }

    ///-///////////////////////////////////////////////////////////
    /// When an enemy dies near the soul collector gameObject, then 
    /// increment "collectedSouls" by 1. When collectedSouls reaches a threshold,
    /// the objective is complete.
    /// 
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
            RemoveCooldown();
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, collectRange);
    }
}
