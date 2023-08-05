using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDCooldown : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [Header("Roll Cooldown UI")]
    [SerializeField] private Image rollFillImage;
    [SerializeField] private TMP_Text rollCooldownText;
    
    [Header("Utility Cooldown UI")]
    [SerializeField] private Image utilityFillImage;
    [SerializeField] private TMP_Text utilityCooldownText;
    [SerializeField] private TMP_Text utilityUsesText;
    
    [Header("Ultimate Cooldown UI")]
    [SerializeField] private Image ultimateFillImage;
    [SerializeField] private TMP_Text ultimateCooldownText;
    
    private CooldownSystem _playerCooldownSystem;

    private int _rollCooldownId;
    private int _utilityCooldownId;
    private int _ultimateCooldownId;
    
    private void OnEnable()
    {
        playerEvents.givePlayerRollCooldownId += SetRollCooldownId;
        playerEvents.giveUtilityCooldownId += SetUtilityCooldownId;
        playerEvents.giveUltimateCooldownId += SetUltimateCooldownId;
        playerEvents.onPlayerUtilityUsesUpdated += ModifyUtilityUses;
    }

    private void OnDisable()
    {
        playerEvents.givePlayerRollCooldownId -= SetRollCooldownId;
        playerEvents.giveUtilityCooldownId -= SetUtilityCooldownId;
        playerEvents.giveUltimateCooldownId -= SetUltimateCooldownId;
    }

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        
        _playerCooldownSystem = player.GetComponent<CooldownSystem>();
    }

    private void Update()
    {
        RollFill();
        UtilityFill();
        UltimateFill();

    }

    private void RollFill()
    {
        if (_playerCooldownSystem.IsOnCooldown(_rollCooldownId))
        {
            float rollCooldownTime = _playerCooldownSystem.GetRemainingDuration(_rollCooldownId) /
                                     _playerCooldownSystem.GetStartingDuration(_rollCooldownId);
            if (rollCooldownTime > 0f)
            {
                rollFillImage.fillAmount = rollCooldownTime;
                rollCooldownText.text = _playerCooldownSystem.GetRemainingDuration(_rollCooldownId).ToString("F1");
            }
        }
        
        else
        {
            rollCooldownText.text = "";
            rollFillImage.fillAmount = 0f;
        }

        
    }

    private void UtilityFill()
    {
        if (_playerCooldownSystem.IsOnCooldown(_utilityCooldownId))
        {
            float utilityCooldownTime = _playerCooldownSystem.GetRemainingDuration(_utilityCooldownId) /
                                        _playerCooldownSystem.GetStartingDuration(_utilityCooldownId);
            if (utilityCooldownTime > 0f)
            {
                utilityFillImage.fillAmount = utilityCooldownTime;
                utilityCooldownText.text = _playerCooldownSystem.GetRemainingDuration(_utilityCooldownId).ToString("F1");
            }
        }
        
        else
        {
            utilityCooldownText.text = "";
            utilityFillImage.fillAmount = 0f;
        }
       
    }

    private void UltimateFill()
    {
        if (_playerCooldownSystem.IsOnCooldown(_ultimateCooldownId))
        {
            float ultimateCooldownTime = _playerCooldownSystem.GetRemainingDuration(_ultimateCooldownId) /
                                         _playerCooldownSystem.GetStartingDuration(_ultimateCooldownId);
            if (ultimateCooldownTime > 0f)
            {
                ultimateFillImage.fillAmount = ultimateCooldownTime;
                ultimateCooldownText.text = _playerCooldownSystem.GetRemainingDuration(_ultimateCooldownId).ToString("F1");
            } 
        }
        
        else
        {
            ultimateCooldownText.text = "";
            ultimateFillImage.fillAmount = 0f;
        }
        
    }

    private void ModifyUtilityUses(int uses)
    {
        utilityUsesText.text = uses.ToString();
    }

    private void SetRollCooldownId(int id)
    {
        _rollCooldownId = id;
    }
    private void SetUtilityCooldownId(int id)
    {
        _utilityCooldownId = id;
    }
    private void SetUltimateCooldownId(int id)
    {
        _ultimateCooldownId = id;
    }
    
}
