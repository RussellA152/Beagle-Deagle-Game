using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// Player must grab an item and walk to a drop off location to complete the objective.
/// This objective will fail if the player does not reach the location in time.
public class DeliverItemObjective : MapObjective
{
    private DropOffLocation _dropOffLocation;
    
    private void Update()
    {
        if(_dropOffLocation.PlayerArrived)
            OnObjectiveCompletion();
    }

    protected override void OnObjectiveAwake()
    {
        base.OnObjectiveAwake();
        _dropOffLocation = GetComponentInChildren<DropOffLocation>();
    }

    protected override void OnObjectiveEnable()
    {
        base.OnObjectiveEnable();
        _dropOffLocation.gameObject.SetActive(false);
    }

    protected override void OnObjectiveEnter()
    {
        base.OnObjectiveEnter();
        _dropOffLocation.gameObject.SetActive(true);
    }
}
