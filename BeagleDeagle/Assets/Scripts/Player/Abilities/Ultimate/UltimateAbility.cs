using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class UltimateAbility<T> : MonoBehaviour, IUltimateUpdatable, IHasCooldown, IHasInput where T: UltimateAbilityData
{
    [SerializeField] protected PlayerEvents playerEvents;
    [SerializeField] private SoundEvents soundEvents;

    [SerializeField] private PlayerData playerData;
    protected T UltimateAbilityData;

    private CooldownSystem _cooldownSystem;
    
    private PlayerInput _playerInput;

    private InputAction _ultimateInputAction;

    private bool _canUseUltimate = true;
    
    [Header("Modifiers")]
    [SerializeField, NonReorderable] private List<UltimateCooldownModifier> ultimateCooldownModifiers = new List<UltimateCooldownModifier>();

    private float _bonusUltimateCooldown = 1f;

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
        CooldownDuration = UltimateAbilityData.cooldown * _bonusUltimateCooldown;
        
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

            StartCoroutine(StartDelay());

        }
    }
    protected abstract void UltimateAction();

    // Wait a few seconds to activate ultimate ability
    // Play activation sound before delay
    private IEnumerator StartDelay()
    {
        PlayActivationSound();
        yield return new WaitForSeconds(UltimateAbilityData.startDelay);
        UltimateAction();
    }
    
    private void PlayActivationSound()
    {
        if(UltimateAbilityData.activationSound != null)
            soundEvents.InvokeGeneralSoundPlay(UltimateAbilityData.activationSound, UltimateAbilityData.activationSoundVolume);
    }
    
    protected void StartCooldown()
    {
        _cooldownSystem.PutOnCooldown(this);
    }

    public void AllowUltimate(bool boolean)
    {
        _canUseUltimate = boolean;
    }

    public void AddUltimateCooldownModifier(UltimateCooldownModifier modifierToAdd)
    {
        ultimateCooldownModifiers.Add(modifierToAdd);
        _bonusUltimateCooldown += (_bonusUltimateCooldown * modifierToAdd.bonusUltimateCooldown);
        
        CooldownDuration = UltimateAbilityData.cooldown * _bonusUltimateCooldown;
    }

    public void RemoveUltimateCooldownModifier(UltimateCooldownModifier modifierToRemove)
    {
        ultimateCooldownModifiers.Remove(modifierToRemove);
        _bonusUltimateCooldown /= (1 + modifierToRemove.bonusUltimateCooldown);
        
        CooldownDuration = UltimateAbilityData.cooldown * _bonusUltimateCooldown;
    }

    public virtual void UpdateScriptableObject(UltimateAbilityData scriptableObject)
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

