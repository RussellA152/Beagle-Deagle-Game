using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostOnKill : PassiveAbility, IHasCooldown
{
    [SerializeField] private MovementSpeedBoostData movementSpeedBoostData;

    private CooldownSystem _cooldownSystem;
    private ModifierManager _modifierManager;

    [SerializeField, Range(1, 10)] 
    private int killsRequired; // How many kills are needed to receive a speed boost?

    [SerializeField, Range(1f, 10f)] 
    private float resetKillCountTimer; // How long does it take to reset kills back to 0?
    
    [SerializeField, Range(0.5f, 15f)] 
    private float movementSpeedBoostDuration; // How long does the movement speed boost on kill last?
    
    // How many enemies have been killed before getting reset every 'x' seconds?
    private int _enemyKillCount;

    private bool _playerHasSpeedBoost = false; // Does the player have the speed boost (it should not stack)

    protected override void OnEnable()
    {
        _cooldownSystem = Player.GetComponent<CooldownSystem>();
        _modifierManager = Player.GetComponent<ModifierManager>();
    }

    protected override void ActivatePassive()
    {
        Id = _cooldownSystem.GetAssignableId();
        
        CooldownDuration = resetKillCountTimer;

        _cooldownSystem.OnCooldownEnded += ResetKillCount;
            
        EnemyManager.Instance.onEnemyDeathGiveGameObject += CountEnemyKill;

        _modifierManager.onModifierWasRemoved += SpeedBoostExpired;
    }

    protected override void RemovePassive()
    {
        _cooldownSystem.OnCooldownEnded -= ResetKillCount;
        
        EnemyManager.Instance.onEnemyDeathGiveGameObject -= CountEnemyKill;
        
        _modifierManager.onModifierWasRemoved -= SpeedBoostExpired;
    }

    private void CountEnemyKill(GameObject enemyGameObject)
    {
        // Start a timer that will reset the amount of kills required back to 0
        if(!_cooldownSystem.IsOnCooldown(Id))
            _cooldownSystem.PutOnCooldown(this);
        
        _enemyKillCount++;
        
        CheckIfPlayerMetKillRequirement();
        
        Debug.Log("Enemies killed so far: " + _enemyKillCount);
    }

    private void CheckIfPlayerMetKillRequirement()
    {
        // If the player reaches the kill requirement (ex. 3 kills or every 3 kills), give them the movement speed boost they need
        if (_enemyKillCount % killsRequired  == 0)
        {
            // If player doesn't have the speed boost, give it to them for the first time
            if (!_playerHasSpeedBoost)
            {
                _modifierManager.AddModifierOnlyForDuration(movementSpeedBoostData.movementSpeedModifier, movementSpeedBoostDuration);
                _playerHasSpeedBoost = true;
            
                DisplayPassiveOnBuffBar();
            }
            // If player already has the speed boost and meets kill requirements again, then refresh the timer on their movement speed boost
            else
            {
                _cooldownSystem.RefreshCooldown(Id);
                _modifierManager.RefreshTimerOnRemoveModifier(movementSpeedBoostData.movementSpeedModifier, movementSpeedBoostDuration);
            }
            
            
        }
    }

    private void ResetKillCount(int id)
    {
        if (Id != id) return;

        _enemyKillCount = 0;
        Debug.Log("Enemies killed was reset. Enemies killed: " + _enemyKillCount);
    }

    ///-///////////////////////////////////////////////////////////
    /// Check if a removed modifier was the same as the movement speed boost given by this script.
    /// If so, then the player no longer has the speed boost, and we can give the boost again (prevents stacking).
    /// 
    private void SpeedBoostExpired(Modifier modifierRemoved)
    {
        if (modifierRemoved == movementSpeedBoostData.movementSpeedModifier)
        {
            _playerHasSpeedBoost = false;
            RemovePassiveFromBuffBar();
            
            Debug.Log("Speed boost on kill was removed!");
        }
            
    }

    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
