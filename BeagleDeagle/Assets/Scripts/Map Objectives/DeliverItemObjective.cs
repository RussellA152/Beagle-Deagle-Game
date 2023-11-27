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
    [SerializeField] private PlayerEvents playerEvents;
    
    [SerializeField] private List<Transform> potentialDropOffLocations = new List<Transform>();
    [SerializeField] private DropOffItem dropOffItem;

    // How long does it take for the player to pick up the deliverable item?
    [SerializeField, Range(0.1f, 1.5f)] 
    private float pickUpTimer = 0.8f;
    private float _playerPickUpTimer;

    [SerializeField, Range(1, 5)] 
    private int numberOfHitsBeforeDropping = 1;
    private int _hitsLeftBeforeDropping;
    
    private DropOffItem _currentDropOff;

    private Waypoint_Indicator _waypointIndicator;

    private bool _playerHoldingItem = false;

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

        _playerPickUpTimer = pickUpTimer;
        _hitsLeftBeforeDropping = numberOfHitsBeforeDropping;

        // Show waypoint indicator sprite and text when objective starts
        _waypointIndicator.enableSprite = true;
        _waypointIndicator.enableText = true;

        playerEvents.onPlayerTookDamage += DropDeliverItemOnDamage;

    }

    protected override void OnObjectiveUpdate()
    {
        base.OnObjectiveUpdate();

        if (PlayerInsideStartingArea && !_playerHoldingItem)
        {
            _playerPickUpTimer -= Time.deltaTime;
            
            if (_playerPickUpTimer <= 0f)
            {
                _playerHoldingItem = true;
                _currentDropOff.playerHoldingItem = true;

                _hitsLeftBeforeDropping = numberOfHitsBeforeDropping;
            }
        }

        if(!PlayerInsideStartingArea)
        {
            _playerPickUpTimer = pickUpTimer;
        }


        if (_playerHoldingItem)
        {
            transform.position = PlayerTransform.position;
        }
        

        if (IsActive && _currentDropOff.PlayerArrived)
        {
            OnObjectiveCompletion();
            RemoveCooldown();
        }
        
    }

    protected override void OnObjectiveEnter()
    {
        base.OnObjectiveEnter();

        if(_currentDropOff == null)
            PickRandomDropOffLocation();

        // Stop showing waypoint indicator sprite and text when objective starts
        _waypointIndicator.enableSprite = false;
        _waypointIndicator.enableText = false;
    }

    private void DropDeliverItemOnDamage()
    {
        if (IsActive)
        {
            _hitsLeftBeforeDropping--;
        
            if (_hitsLeftBeforeDropping <= 0)
            {
                _playerHoldingItem = false;
        
                _playerPickUpTimer = pickUpTimer;
        
                _currentDropOff.playerHoldingItem = false;
            }
        }
        
        
    }

    ///-///////////////////////////////////////////////////////////
    /// Find a random location to spawn a DropOffItem at.
    /// 
    private void PickRandomDropOffLocation()
    {
        int randomLocationIndex = Random.Range(0, potentialDropOffLocations.Count);

         GameObject newDropOffGameObject =  Instantiate(dropOffItem.gameObject);

         _currentDropOff = newDropOffGameObject.GetComponent<DropOffItem>();
        _currentDropOff.transform.position = potentialDropOffLocations[randomLocationIndex].position;
        
    }

    public override string GetObjectiveDescription()
    {
        if (!_playerHoldingItem && IsActive)
            return "Item Not Retrieved!";
        
        return "Deliver The Item!";
    }
}
