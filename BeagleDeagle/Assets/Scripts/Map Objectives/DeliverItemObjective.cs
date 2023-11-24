using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

///-///////////////////////////////////////////////////////////
/// Player must grab an item and walk to a drop off location to complete the objective.
/// This objective will fail if the player does not reach the location in time.
public class DeliverItemObjective : MapObjective
{
    [SerializeField] private List<Transform> potentialDropOffLocations = new List<Transform>();
    [SerializeField] private DropOffItem dropOffItem;
    private DropOffItem _currentDropOff;

    private Waypoint_Indicator _waypointIndicator;

    private bool _playerHoldingItem = false;

    [SerializeField] private Transform playerTransform;

    protected override void OnObjectiveAwake()
    {
        base.OnObjectiveAwake();
        _waypointIndicator = GetComponent<Waypoint_Indicator>();
    }

    protected override void OnObjectiveDisable()
    {
        if(_currentDropOff != null)
            Destroy(_currentDropOff.gameObject);
        
        _playerHoldingItem = false;
        
        base.OnObjectiveDisable();
        
    }

    protected override void OnObjectiveEnable()
    {
        base.OnObjectiveEnable();

        // Show waypoint indicator sprite and text when objective starts
        _waypointIndicator.enableSprite = true;
        _waypointIndicator.enableText = true;
    }

    protected override void OnObjectiveUpdate()
    {
        base.OnObjectiveUpdate();
        
        
        if (_playerHoldingItem)
            transform.position = playerTransform.position;
        
        if(IsActive &&  _currentDropOff.PlayerArrived)
            OnObjectiveCompletion();
    }

    protected override void OnObjectiveEnter()
    {
        base.OnObjectiveEnter();

        PickRandomDropOffLocation();
        
        _playerHoldingItem = true;
        
        // Stop showing waypoint indicator sprite and text when objective starts
        _waypointIndicator.enableSprite = false;
        _waypointIndicator.enableText = false;
    }

    private void PickRandomDropOffLocation()
    {
        int randomLocationIndex = Random.Range(0, potentialDropOffLocations.Count);

         GameObject newDropOffGameObject =  Instantiate(dropOffItem.gameObject);

         _currentDropOff = newDropOffGameObject.GetComponent<DropOffItem>();
        _currentDropOff.transform.position = potentialDropOffLocations[randomLocationIndex].position;

        _currentDropOff.playerHoldingItem = true;
    }

    public override string GetObjectiveDescription()
    {
        return "Deliver The Item!";
    }
}
