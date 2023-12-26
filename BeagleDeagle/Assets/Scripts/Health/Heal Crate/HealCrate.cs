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
    private ConsumablePickUp _healthPickUp;

    private CooldownSystem _cooldownSystem;
    [SerializeField, Range(0.1f, 120f)] 
    private float restoreTimer = 5f;

    private void Awake()
    {
        _healthScript = crateGameObject.GetComponent<ObjectHealth>();
        _healthPickUp = healItemGameObject.GetComponent<ConsumablePickUp>();
        _cooldownSystem = GetComponent<CooldownSystem>();
    }

    private void Start()
    {
        Id = 20;
        CooldownDuration = restoreTimer;
        
        crateGameObject.SetActive(true);
        healItemGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _healthScript.onDeath += SpawnCrate;
        _healthPickUp.onPickUpDespawn += StartCooldown;
        _cooldownSystem.OnCooldownEnded += RestoreCrate;
    }

    private void OnDisable()
    {
        _healthScript.onDeath -= SpawnCrate;
        _healthPickUp.onPickUpDespawn -= StartCooldown;
        _cooldownSystem.OnCooldownEnded -= RestoreCrate;
    }


    ///-///////////////////////////////////////////////////////////
    /// After being destroyed, spawn the consumable for someone to pick up.
    /// 
    private void SpawnCrate()
    {
        crateGameObject.SetActive(false);
        healItemGameObject.SetActive(true);
    }

    private void StartCooldown()
    {
        _cooldownSystem.PutOnCooldown(this);
    }

    ///-///////////////////////////////////////////////////////////
    /// After being destroyed, the crate will restore itself and become destructible once again.
    /// 
    private void RestoreCrate(int id)
    {
        if (Id != id) return;
        
        crateGameObject.SetActive(true);
        healItemGameObject.SetActive(false);
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
