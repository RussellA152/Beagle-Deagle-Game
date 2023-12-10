using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DropOffItem : MonoBehaviour
{
    [SerializeField] private LayerMask layersThatCanDropOff;
    
    // How close does the player need to be (while holding deliver item) to drop it off here
    [SerializeField,Range(1f, 100f)] private float dropOffRange;
    
    // Did the player reach this location?
    public bool PlayerArrived { get; private set; }

    // Does the player have the deliver item?
    [HideInInspector] 
    public bool playerHoldingItem = false;

    private RangeIndicator _rangeIndicator;

    private void Awake()
    {
        _rangeIndicator = GetComponentInChildren<RangeIndicator>();
    }

    private void Start()
    {
        PlayerArrived = false;
        playerHoldingItem = false;
        
        DisplayRangeIndicator();
    }
    
    private void Update()
    {
        // Check if the player is holding the deliver item and in dropOffRange
        if (playerHoldingItem && Physics2D.OverlapCircle(transform.position, dropOffRange, layersThatCanDropOff))
        {
            PlayerArrived = true;
        }
    }
    
    private void DisplayRangeIndicator()
    {
        // Display the exiting range of this map objective
        _rangeIndicator.gameObject.SetActive(true);
        
        _rangeIndicator.SetSize(new Vector2(dropOffRange, dropOffRange));
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, dropOffRange);
    }
}
