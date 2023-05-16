using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    private PlayerInput playerInput;

    private GameObject player;

    //private IWeapon playerCurrentWeapon;

    [TagSelector] public string playerTag;
    
    private void Awake()
    {
        if(instance != null & instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag(playerTag);

        playerInput = player.GetComponent<PlayerInput>();

        //playerCurrentWeapon = player.GetComponentInChildren<IWeapon>();
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    //public IWeapon GetPlayerWeapon()
    //{
    //    return playerCurrentWeapon;
    //}

    public PlayerInput GetPlayerInput()
    {
        return playerInput;
    }
}
