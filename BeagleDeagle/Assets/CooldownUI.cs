using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [Header("Roll Cooldown UI")]
    [SerializeField] private Image rollFillImage;

    [SerializeField] private Image rollFillBorder;

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

    // Duration that the roll progression bar has been on the screen (after roll cooldown finishes)
    private float _rollFillDisplayTime;
    
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
        
        // Don't show roll progression until player rolls for the first time
        rollFillImage.enabled = false;
        rollFillBorder.enabled = false;
    }

    private void Update()
    {
        HideRollFill();
        
        RollFill();
        UtilityFill();
        UltimateFill();
    }

    #region Roll Cooldown
    private void HideRollFill()
    {
        _rollFillDisplayTime += Time.deltaTime;
        
        if (rollFillImage.fillAmount <= 0f) {
            if (_rollFillDisplayTime >= 0.75f)
            {
                rollFillImage.enabled = false;
                rollFillBorder.enabled = false;
            }
        }
        else
        {
            _rollFillDisplayTime = 0f;
        }
    }

    private void RollFill()
    {
        if (_playerCooldownSystem.IsOnCooldown(_rollCooldownId))
        {
            rollFillImage.enabled = true;
            rollFillBorder.enabled = true;
            
            float rollCooldownTime = _playerCooldownSystem.GetRemainingDuration(_rollCooldownId) /
                                     _playerCooldownSystem.GetStartingDuration(_rollCooldownId);
            if (rollCooldownTime > 0f)
            {
                rollFillImage.fillAmount = rollCooldownTime;
            }
        }
        
        else
        {
            rollFillImage.fillAmount = 0f;
        }
        
    }
    
    private void SetRollCooldownId(int id)
    {
        _rollCooldownId = id;
    }

    #endregion

    #region UtilityCooldown
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
    
    private void ModifyUtilityUses(int uses)
    {
        utilityUsesText.text = uses.ToString();
    }

    
    private void SetUtilityCooldownId(int id)
    {
        _utilityCooldownId = id;
    }

    #endregion

    #region UltimateCooldown
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
    private void SetUltimateCooldownId(int id)
    {
        _ultimateCooldownId = id;
    }

    #endregion

}
