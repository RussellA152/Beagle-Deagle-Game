using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveInventory : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    
    // The passive that this character will start with
    private PassiveAbilityData _startingPassive;

    private void Awake()
    {
        _startingPassive = playerData.passiveAbilityData;
        
        GetNewPassive(_startingPassive);
    }

    ///-///////////////////////////////////////////////////////////
    /// Receive and crate a new passive ability for this character (these should work for both characters)
    /// 
    public void GetNewPassive(PassiveAbilityData newPassive)
    {
        Instantiate(newPassive.gameObjectWithPassive, Vector3.zero, Quaternion.identity, transform);
    }
}
