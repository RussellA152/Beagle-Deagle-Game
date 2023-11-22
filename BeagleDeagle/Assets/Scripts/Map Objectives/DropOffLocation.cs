using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffLocation : MonoBehaviour
{
    // Did the player reach this location?
    public bool PlayerArrived { get; private set; }
    
    private void OnDisable()
    {
        PlayerArrived = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerArrived = true;
        }
    }
}
