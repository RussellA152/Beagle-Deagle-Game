using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// This script will ask the MapObjectiveManager to remove the map objective if the player has ignored it for too long.
/// If the player activates the map objective, the expire timer will stop.
public class MapObjectiveExpire : MonoBehaviour, IHasCooldown
{
    // How long can this objective be ignored for?
    [SerializeField, Range(1f, 180f)] 
    private float timeUntilExpire;

    private CooldownSystem _cooldownSystem;
    
    private void Awake()
    {
        Id = 40;
        _cooldownSystem = GetComponent<CooldownSystem>();
        CooldownDuration = timeUntilExpire;

    }

    private void Start()
    {
        _cooldownSystem.OnCooldownEnded += ObjectiveExpired;
    }

    private void OnEnable()
    {
        _cooldownSystem.PutOnCooldown(this);
    }

    private void OnDestroy()
    {
        _cooldownSystem.OnCooldownEnded -= ObjectiveExpired;
    }
    
    private void ObjectiveExpired(int cooldownId)
    {
        if (Id == cooldownId)
        {
            MapObjectiveManager.Instance.StartNewObjectiveAfterExpired(this);
        }
           
    }

    ///-///////////////////////////////////////////////////////////
    /// Removes expire timer when the player activates the objective (called by MapObjective)
    ///  
    public void RemoveExpireTimeOnActivate()
    {
        _cooldownSystem.RemoveCooldown(Id);
    }
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
