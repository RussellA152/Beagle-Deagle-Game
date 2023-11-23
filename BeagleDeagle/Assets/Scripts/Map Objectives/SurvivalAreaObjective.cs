using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalAreaObjective : MapObjective
{
    // How much time does the player need to stay inside the area for?
    [SerializeField, Range(10f, 60f)] 
    private float timeRequiredToStayInside = 30f;
    
    // How much time has the player spent inside of the survival area?
    private float _elapsedTimeInsideArea = 0f;
    
    // Is the player inside of the survival area?
    private bool _playerInsideArea = false;

    private void Update()
    {
        // While player is inside the survival area, then add time to the timeSpentInsideArea
        if (_playerInsideArea)
            _elapsedTimeInsideArea += Time.deltaTime;
        
        // Complete objective once player has spent enough time inside survival area
        if (_elapsedTimeInsideArea >= timeRequiredToStayInside)
        {
            OnObjectiveCompletion();
            RemoveCooldown();
        }

    }

    protected override void OnObjectiveDisable()
    {
        base.OnObjectiveDisable();
        
        _elapsedTimeInsideArea = 0f;
        _playerInsideArea = false;
    }

    protected override void OnObjectiveEnter()
    {
        base.OnObjectiveEnter();
        _playerInsideArea = true;
    }

    public override string GetObjectiveDescription()
    {
        return "Survive: " + (int) _elapsedTimeInsideArea + "s";
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Player is no longer inside of the survival area
            _playerInsideArea = false;
        }
    }
}
