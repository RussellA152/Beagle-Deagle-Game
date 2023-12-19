using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class UltimateAbility<T> : MonoBehaviour, IUltimateUpdatable, IHasCooldown, IHasInput where T: UltimateAbilityData
{
    [SerializeField] protected PlayerEvents playerEvents;

    [SerializeField] private PlayerData playerData;
    protected T UltimateAbilityData;

    private CooldownSystem _cooldownSystem;
    
    private PlayerInput _playerInput;

    private InputAction _ultimateInputAction;

    private bool _canUseUltimate = true;

    private void Awake()
    {
        //_topDownInput = new TopDownInput();
        _playerInput = GetComponent<PlayerInput>();
        _cooldownSystem = GetComponent<CooldownSystem>();
        _ultimateInputAction = _playerInput.currentActionMap.FindAction("Ultimate");
        
        UltimateAbilityData = (T) playerData.ultimateAbilityData;
        
    }

    protected virtual void OnEnable()
    {
        _ultimateInputAction.performed += ActivateUltimate;
    }
    
    protected virtual void OnDisable()
    {
        _ultimateInputAction.performed -= ActivateUltimate;
        StopAllCoroutines();
    }
    
    protected virtual void Start()
    {

        Id = 10;
        CooldownDuration = UltimateAbilityData.cooldown;
        
        playerEvents.InvokeUltimateCooldown(Id);
        playerEvents.InvokeNewUltimate(UltimateAbilityData);
    }

    ///-///////////////////////////////////////////////////////////
    /// When player presses 'Q' or ultimate button, 
    /// activate the ultimate ability associated with the character
    public void ActivateUltimate(CallbackContext context)
    {
        // If the ultimate is not on cooldown, and the player is allowed to use ultimate ("if player is not in a state that disables ultimate
        // like the 'Dead' state")
        if (!_cooldownSystem.IsOnCooldown(Id) && _canUseUltimate)
        {
            Debug.Log("Activate ultimate!");

            UltimateAction();

        }
    }
    protected abstract void UltimateAction();
    
    protected void StartCooldown()
    {
        _cooldownSystem.PutOnCooldown(this);
    }

    public void AllowUltimate(bool boolean)
    {
        _canUseUltimate = boolean;
    }
    
    public void UpdateScriptableObject(UltimateAbilityData scriptableObject)
    {
        if (scriptableObject is T)
        {
            UltimateAbilityData = scriptableObject as T;
            playerEvents.InvokeNewUltimate(UltimateAbilityData);
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
    public void AllowInput(bool boolean)
    {
        if (boolean)
        {
            _ultimateInputAction.Enable();
        }
        else
        {
            _ultimateInputAction.Disable();
        }
    }
}

