using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DefendObjective : MapObjective
{
    private ObjectHealth _objectWithHealth;

    private Transform _previousEnemyTarget;
    
    // How much time does the player need to stay inside the area for?
    [SerializeField, Range(10f, 60f)] 
    private float timeRequiredToDefend = 30f;

    // How much time does the player need left to complete?
    private float _timeNeededLeft;

    protected override void OnObjectiveAwake()
    {
        base.OnObjectiveAwake();
        
        _objectWithHealth = GetComponentInChildren<ObjectHealth>();
    }

    protected override void OnObjectiveUpdate()
    {
        base.OnObjectiveUpdate();
        
        // While player is inside the survival area, then add time to the timeSpentInsideArea
        if (PlayerInsideStartingArea)
            _timeNeededLeft -= Time.deltaTime;
        
        
        if(_timeNeededLeft <= 0f)
            OnObjectiveOutOfTime();
    }

    protected override void OnObjectiveEnable()
    {
        base.OnObjectiveEnable();
        
        // Defend object is not enabled until objective has started
        _objectWithHealth.gameObject.SetActive(false);

        _objectWithHealth.onDeath += OnObjectiveOutOfTime;
        
        _previousEnemyTarget = EnemyManager.Instance.GetGlobalTarget();
        
        _timeNeededLeft = timeRequiredToDefend;
        
    }

    protected override void OnObjectiveEnter()
    {
        base.OnObjectiveEnter();
        
        _objectWithHealth.gameObject.SetActive(true);
        
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

    public override string GetObjectiveDescription()
    {
        return "Defend: " + (int) _timeNeededLeft + "s";
    }
}
