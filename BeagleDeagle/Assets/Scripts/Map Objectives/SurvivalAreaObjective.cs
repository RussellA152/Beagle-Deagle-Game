using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalAreaObjective : MapObjective
{
    // How much time does the player need to stay inside the area for?
    [SerializeField, Range(10f, 60f)] 
    private float timeRequiredToStayInside = 30f;

    // How much time does the player need left to complete?
    private float _timeNeededLeft;

    protected override void OnObjectiveEnable()
    {
        base.OnObjectiveEnable();
        _timeNeededLeft = timeRequiredToStayInside;
    }

    protected override void OnObjectiveUpdate()
    {
        base.OnObjectiveUpdate();
        
        // While player is inside the survival area, then add time to the timeSpentInsideArea
        if (PlayerInsideArea)
            _timeNeededLeft -= Time.deltaTime;
        
        // Complete objective once player has spent enough time inside survival area
        if (_timeNeededLeft <= 0f)
        {
            OnObjectiveCompletion();
            RemoveCooldown();
        }
    }

    protected override void OnObjectiveEnter()
    {
        base.OnObjectiveEnter();
        PlayerInsideArea = true;
    }
    

    public override string GetObjectiveDescription()
    {
        if (!PlayerInsideArea)
            return "Return To Survival Area!";
        
        return "Survive: " + (int) _timeNeededLeft + "s";
    }

}
