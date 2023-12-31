using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ReloadFireRateBoost : PassiveAbility, IHasCooldown
{
    [SerializeField] private PlayerEvents playerEvents;
    
    private IGunDataUpdatable _gunDataUpdatable;
    private CooldownSystem _cooldownSystem;
    
    [SerializeField] private AttackSpeedBoostData attackSpeedBoost;
    
    [SerializeField, Range(1f, 10f)] 
    private float duration;

    private bool _playerHasFireRateBoost;
    

    protected override void Awake()
    {
        base.Awake();
        
        _gunDataUpdatable = Player.GetComponentInChildren<IGunDataUpdatable>();
        _cooldownSystem = Player.GetComponent<CooldownSystem>();
    }

    private void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = duration;
    }

    protected override void ActivatePassive()
    {
        // Wait for player to finish reloading, then give fire rate boost
        playerEvents.onPlayerReloadFinished += GiveFireRateBoost;
        
        // Remove fire rate boost after a few seconds
        _cooldownSystem.OnCooldownEnded += RemoveFireRateBoost;
    }

    protected override void RemovePassive()
    {
        playerEvents.onPlayerReloadFinished -= GiveFireRateBoost;
        _cooldownSystem.OnCooldownEnded -= RemoveFireRateBoost;
        
        // Force remove fire rate boost
        _cooldownSystem.EndCooldown(Id);
    }

    private void GiveFireRateBoost()
    {
        if (!_playerHasFireRateBoost)
        {
            _gunDataUpdatable.AddAttackSpeedModifier(attackSpeedBoost.attackSpeedModifier);
            _cooldownSystem.PutOnCooldown(this);
            _playerHasFireRateBoost = true;
        }
    }

    private void RemoveFireRateBoost(int id)
    {
        if (Id == id && _playerHasFireRateBoost)
        {
            _gunDataUpdatable.RemoveAttackSpeedModifier(attackSpeedBoost.attackSpeedModifier);
            _playerHasFireRateBoost = false;
        }
        
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
