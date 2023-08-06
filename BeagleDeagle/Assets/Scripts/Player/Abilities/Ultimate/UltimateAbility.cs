using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class UltimateAbility<T> : MonoBehaviour, IUltimateUpdatable, IHasCooldown where T: UltimateAbilityData
{
    [SerializeField] protected PlayerEvents playerEvents;
    
    [SerializeField] protected T ultimateData;

    public CooldownSystem CooldownSystem;
    
    private TopDownInput _topDownInput;

    private InputAction _ultimateInputAction;
    
    private bool _canUseUltimate;

    private void Awake()
    {
        _topDownInput = new TopDownInput();
        CooldownSystem = GetComponent<CooldownSystem>();
        _ultimateInputAction = _topDownInput.Player.Ultimate;

        Id = 10;
        CooldownDuration = ultimateData.cooldown;
    }

    protected virtual void OnEnable()
    {
        _topDownInput.Enable();

        _ultimateInputAction.performed += ActivateUltimate;
    }
    
    protected virtual void OnDisable()
    {
        _topDownInput.Disable();
        _ultimateInputAction.performed -= ActivateUltimate;
        StopAllCoroutines();
    }
    
    protected virtual void Start()
    {
        _canUseUltimate = false;
        
        playerEvents.InvokeUltimateCooldown(Id);
        playerEvents.InvokeNewUltimate(ultimateData);
    }

    ///-///////////////////////////////////////////////////////////
    /// When player presses 'Q' or ultimate button, 
    /// activate the ultimate ability associated with the character
    public void ActivateUltimate(CallbackContext context)
    {
        if (!CooldownSystem.IsOnCooldown(Id))
        {
            Debug.Log("Activate ultimate!");

            UltimateAction(gameObject);

        }
    }
    protected abstract void UltimateAction(GameObject player);
    
    protected void StartCooldown()
    {
        CooldownSystem.PutOnCooldown(this);
    }

    public void AllowUltimate(bool boolean)
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
    public void UpdateScriptableObject(UltimateAbilityData scriptableObject)
    {
        if (scriptableObject is T)
        {
            ultimateData = scriptableObject as T;
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}

