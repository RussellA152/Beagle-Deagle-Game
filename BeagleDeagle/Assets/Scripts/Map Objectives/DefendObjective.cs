using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendObjective : MapObjective
{
    private ObjectHealth _objectWithHealth;

    private Transform _previousEnemyTarget;

    private void Update()
    {
        if(_objectWithHealth.IsDead())
            OnObjectiveOutOfTime();
    }

    protected override void OnObjectiveAwake()
    {
        base.OnObjectiveAwake();
        
        _objectWithHealth = GetComponentInChildren<ObjectHealth>();
    }

    protected override void OnObjectiveEnter()
    {
        base.OnObjectiveEnter();
        
        _previousEnemyTarget = EnemyManager.Instance.GetGlobalTarget();
        
        // Change all enemy targets to be this defended object
        EnemyManager.Instance.ChangeAllEnemyTarget(_objectWithHealth.transform);
    }

    protected override void OnObjectiveOutOfTime()
    {
        // Set enemy targets back to whatever it was before
        EnemyManager.Instance.ChangeAllEnemyTarget(_previousEnemyTarget);
        
        // When the objective timer ends, check if the object to defend is still alive.
        // If so, the player successfully completed this objective, otherwise they failed to defend the object.
        if (!_objectWithHealth.IsDead())
        {
            // * Do not remove cooldown because cooldown is already removed *
            OnObjectiveCompletion();
        }
        else
        {
            base.OnObjectiveOutOfTime();
        }
        
    }
}
