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
        
        // Reset number of collected souls
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
        }

        if (_collectedSouls >= collectRequirement)
        {
            OnObjectiveCompletion();
            RemoveCooldown();
        }
    }

    protected override void DisplayRangeIndicator()
    {
        base.DisplayRangeIndicator();
        
        RangeIndicator.SetSize(new Vector2(collectRange, collectRange));
    }

    public override string GetObjectiveDescription()
    {
        return "Souls: " + _collectedSouls + " of " + collectRequirement;
    }
}
