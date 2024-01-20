using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HealCrate : MonoBehaviour, IHasCooldown
{
    [SerializeField] private GameObject crateGameObject;
    [SerializeField] private GameObject healItemGameObject;

    private ObjectHealth _healthScript;
    private PowerUp _powerUp;
    private Waypoint_Indicator _waypointIndicator;

    private CooldownSystem _cooldownSystem;
    
    [SerializeField, Range(0.1f, 120f)] 
    private float restoreTimer = 5f; // How long does it take for this crate to reset itself?

    private void Awake()
    {
        _healthScript = crateGameObject.GetComponent<ObjectHealth>();
        _powerUp = healItemGameObject.GetComponent<PowerUp>();
        _cooldownSystem = GetComponent<CooldownSystem>();
        _waypointIndicator = GetComponent<Waypoint_Indicator>();

    }

    private void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = restoreTimer;
        
        crateGameObject.SetActive(true);
        healItemGameObject.SetActive(false);
        
        _waypointIndicator.enabled = true;
    }

    private void OnEnable()
    {
        _healthScript.onDeath += SpawnCrate;
        _powerUp.onPickUpDeactivate += StartCooldown;
        _cooldownSystem.OnCooldownEnded += RestoreCrate;
    }

    private void OnDisable()
    {
        _healthScript.onDeath -= SpawnCrate;
        _powerUp.onPickUpDeactivate -= StartCooldown;
        _cooldownSystem.OnCooldownEnded -= RestoreCrate;
    }


    ///-///////////////////////////////////////////////////////////
    /// After being destroyed, spawn the consumable for someone to pick up.
    /// 
    private void SpawnCrate()
    {
        crateGameObject.SetActive(false);
        healItemGameObject.SetActive(true);
        
        _powerUp.ActivatePowerUp();
    }

    private void StartCooldown()
    {
        _cooldownSystem.PutOnCooldown(this);
        
        _waypointIndicator.enabled = false;
    }

    ///-///////////////////////////////////////////////////////////
    /// After being destroyed, the crate will restore itself and become destructible once again.
    /// 
    private void RestoreCrate(int id)
    {
        if (Id != id) return;
        
        _waypointIndicator.enabled = true;
        crateGameObject.SetActive(true);
        healItemGameObject.SetActive(false);
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
