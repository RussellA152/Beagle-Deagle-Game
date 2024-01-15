using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ReloadFireRateBoost : PassiveAbility, IHasCooldown
{
    private CooldownSystem _cooldownSystem;
    private ModifierManager _modifierManager;
    
    [SerializeField] private AttackSpeedBoostData attackSpeedBoost;
    
    [SerializeField, Range(1f, 10f)] 
    private float duration;

    private bool _playerHasFireRateBoost;
    

    protected override void Awake()
    {
        base.Awake();
        
        _modifierManager = Player.GetComponent<ModifierManager>();
        _cooldownSystem = Player.GetComponent<CooldownSystem>();
        
        ShowOnBuffBar.SetModifierManager(_modifierManager);
        ShowOnBuffBar.SetBuffModifier(attackSpeedBoost.attackSpeedModifier);
        
    }

    protected override void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = duration;
        
        base.Start();
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
            _modifierManager.AddModifier(attackSpeedBoost.attackSpeedModifier);
            _cooldownSystem.PutOnCooldown(this);
        }
        else
        {
            _cooldownSystem.RefreshCooldown(Id);

        }
        
        _playerHasFireRateBoost = true;   
        ShowOnBuffBar.ShowBuffIconWithDuration(duration);
    }

    private void RemoveFireRateBoost(int id)
    {
        if (Id == id && _playerHasFireRateBoost)
        {
            _modifierManager.RemoveModifier(attackSpeedBoost.attackSpeedModifier);
            _playerHasFireRateBoost = false;
        }
        
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
