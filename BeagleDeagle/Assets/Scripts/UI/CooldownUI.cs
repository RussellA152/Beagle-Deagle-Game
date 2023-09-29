using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Color = System.Drawing.Color;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    private Canvas parentCanvas;
    
    [Header("Reload Cooldown UI")] 
    [SerializeField] private RectTransform reloadFillRecTransform;
    [SerializeField] private Image reloadFillImage;
    [SerializeField] private RectTransform reloadOnGamepadPosition;

    [Header("Roll Cooldown UI")] 
    [SerializeField] private Slider rollSlider;

    [Header("Utility Cooldown UI")]
    [SerializeField] private Image utilityFillImage;
    [SerializeField] private TMP_Text utilityCooldownText;
    [SerializeField] private TMP_Text utilityUsesText;
    
    [Header("Ultimate Cooldown UI")]
    [SerializeField] private Image ultimateFillImage;
    [SerializeField] private TMP_Text ultimateCooldownText;
    
    private CooldownSystem _playerCooldownSystem;

    private int _reloadCooldownId;
    private int _rollCooldownId;
    private int _utilityCooldownId;
    private int _ultimateCooldownId;

    // Duration that the roll progression bar has been on the screen (after roll cooldown finishes)
    private float _rollFillDisplayTime;
    // How long after roll cooldown finishes should the roll fill show on screen? 
    private float _rollFillLingerTime = 0.35f;

    private bool _useMousePosition;

    private void Awake()
    {
        parentCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        playerEvents.givePlayerReloadCooldownId += SetReloadCooldownId;
        playerEvents.givePlayerRollCooldownId += SetRollCooldownId;
        playerEvents.giveUtilityCooldownId += SetUtilityCooldownId;
        playerEvents.giveUltimateCooldownId += SetUltimateCooldownId;
        playerEvents.onPlayerUtilityUsesUpdated += ModifyUtilityUses;

        playerEvents.givePlayerGameObject += GetPlayerCooldownSystem;


    }

    private void OnDisable()
    {
        playerEvents.givePlayerReloadCooldownId -= SetReloadCooldownId;
        playerEvents.givePlayerRollCooldownId -= SetRollCooldownId;
        playerEvents.giveUtilityCooldownId -= SetUtilityCooldownId;
        playerEvents.giveUltimateCooldownId -= SetUltimateCooldownId;
        
        playerEvents.givePlayerGameObject -= GetPlayerCooldownSystem;
    }

    private void Start()
    {
        // Subscribe in Start because OnEnable doesn't work consistently
        PlayerControllerTypeManager.Instance.OnPlayerChangedController += ChangeControllerType;

        // Don't show roll progression until player rolls for the first time
        rollSlider.gameObject.SetActive(false);
        reloadFillRecTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        HideRollFill();
        
        ShowReloadFillOnCursor();
        
        ReloadFill();
        RollFill();
        UtilityFill();
        UltimateFill();
    }

    ///-///////////////////////////////////////////////////////////
    /// Get a reference of the player, then get their CooldownSystem component
    /// so the UI can display their cooldown timers
    /// 
    private void GetPlayerCooldownSystem(GameObject player)
    {
        _playerCooldownSystem = player.GetComponent<CooldownSystem>();
    }

    ///-///////////////////////////////////////////////////////////
    /// If user is playing with keyboard controls, then "_useMousePosition" is set to true, and the reload cooldown progression circle
    /// will be placed on the mouse cursor's position. Otherwise, the progression circle will be placed in a fixed position on the canvas
    /// 
    private void ChangeControllerType(PlayerControllerTypeManager.ControllerType controllerType)
    {
        switch (controllerType)
        {
            case PlayerControllerTypeManager.ControllerType.Keyboard:
                _useMousePosition = true;
                break;
            case PlayerControllerTypeManager.ControllerType.Xbox:
                _useMousePosition = false;
                break;
            case PlayerControllerTypeManager.ControllerType.Playstation:
                _useMousePosition = false;
                break;
        }
    }

    #region Reload Cooldown

    private void ShowReloadFillOnCursor()
    {
        if (_playerCooldownSystem.IsOnCooldown(_reloadCooldownId))
        {
            if (_useMousePosition)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Vector2 uiPosition;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.transform as RectTransform,
                    mousePosition,
                    parentCanvas.worldCamera,
                    out uiPosition);

                reloadFillRecTransform.localPosition = uiPosition;

                Cursor.visible = false;
            }
            else
            {
                reloadFillRecTransform.localPosition = reloadOnGamepadPosition.localPosition;
            }
        }
        else if(_useMousePosition)
        {
            Cursor.visible = true;
        }

    }
    private void ReloadFill()
    {
        if (_playerCooldownSystem.IsOnCooldown(_reloadCooldownId))
        {
            reloadFillRecTransform.gameObject.SetActive(true);
            float reloadCooldownTime = _playerCooldownSystem.GetRemainingDuration(_reloadCooldownId) /
                                       _playerCooldownSystem.GetStartingDuration(_reloadCooldownId);
            if (reloadCooldownTime > 0f)
            {
                reloadFillImage.fillAmount = reloadCooldownTime;
            }
        }
        
        else
        {
            reloadFillRecTransform.gameObject.SetActive(false);
            reloadFillImage.fillAmount = 0f;
        }
    }

    private void SetReloadCooldownId(int id)
    {
        _reloadCooldownId = id;
    }
    

    #endregion
    
    #region Roll Cooldown
    private void HideRollFill()
    {
        _rollFillDisplayTime += Time.deltaTime;
        
        if (rollSlider.value <= 0f) {
            if (_rollFillDisplayTime >= _rollFillLingerTime)
            {
                rollSlider.gameObject.SetActive(false);
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
            rollSlider.gameObject.SetActive(true);
            
            float rollCooldownTime = _playerCooldownSystem.GetRemainingDuration(_rollCooldownId) /
                                     _playerCooldownSystem.GetStartingDuration(_rollCooldownId);
            if (rollCooldownTime > 0f)
            {
                rollSlider.value = rollCooldownTime;
            }
        }
        
        else
        {
            rollSlider.value = 0f;
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
            if (_playerCooldownSystem.GetRemainingDuration(_utilityCooldownId) < 10f)
            {
                utilityCooldownText.text = _playerCooldownSystem.GetRemainingDuration(_utilityCooldownId).ToString("F1");
            }
            else
            {
                utilityCooldownText.text = Mathf.RoundToInt(_playerCooldownSystem.GetRemainingDuration(_utilityCooldownId)).ToString();
            }
            utilityFillImage.fillAmount = utilityCooldownTime;
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
            if (_playerCooldownSystem.GetRemainingDuration(_ultimateCooldownId) < 10f)
            {
                ultimateCooldownText.text = _playerCooldownSystem.GetRemainingDuration(_ultimateCooldownId).ToString("F1");
            }
            else
            {
                ultimateCooldownText.text = Mathf.RoundToInt(_playerCooldownSystem.GetRemainingDuration(_ultimateCooldownId)).ToString();
            }
            
            ultimateFillImage.fillAmount = ultimateCooldownTime;
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
