using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class UltimateAbility<T> : MonoBehaviour, IUltimateUpdatable, IHasCooldown, IHasInput, IRegisterModifierMethods where T: UltimateAbilityData
{
    [SerializeField] protected PlayerEvents playerEvents;
    protected AudioClipPlayer AudioClipPlayer;

    [SerializeField] private PlayerData playerData;
    protected T UltimateAbilityData;

    private CooldownSystem _cooldownSystem;
    protected MiscellaneousModifierList MiscellaneousModifierList;
    protected ModifierManager ModifierManager;
    
    private PlayerInput _playerInput;

    private InputAction _ultimateInputAction;

    private bool _canUseUltimate = true;
    
    [Header("Modifiers")]
    [SerializeField, NonReorderable] private List<UltimateCooldownModifier> ultimateCooldownModifiers = new List<UltimateCooldownModifier>();
    [SerializeField, NonReorderable] private List<UltimateDamageModifier> ultimateDamageModifiers = new List<UltimateDamageModifier>();

    private float _bonusUltimateCooldown = 1f;
    protected float BonusUltimateDamage = 1f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _cooldownSystem = GetComponent<CooldownSystem>();
        AudioClipPlayer = GetComponent<AudioClipPlayer>();
        MiscellaneousModifierList = GetComponent<MiscellaneousModifierList>();

        ModifierManager = GetComponent<ModifierManager>();
        
        _ultimateInputAction = _playerInput.currentActionMap.FindAction("Ultimate");
        
        UltimateAbilityData = (T) playerData.ultimateAbilityData;
        
        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();
        
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

        Id = _cooldownSystem.GetAssignableId();
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
    
    protected virtual void PlayActivationSound()
    {
        AudioClipPlayer.PlayGeneralAudioClip(UltimateAbilityData.activationSound, UltimateAbilityData.activationSoundVolume);
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
        if (ultimateCooldownModifiers.Contains(modifierToAdd)) return;
        
        ultimateCooldownModifiers.Add(modifierToAdd);
        _bonusUltimateCooldown += (_bonusUltimateCooldown * modifierToAdd.bonusUltimateCooldown);
        
        CooldownDuration = UltimateAbilityData.cooldown * _bonusUltimateCooldown;
        
        if (_cooldownSystem.IsOnCooldown(Id))
        {
            _cooldownSystem.ChangeOngoingCooldownTime(Id, CooldownDuration);
        }
    }

    public void RemoveUltimateCooldownModifier(UltimateCooldownModifier modifierToRemove)
    {
        if (!ultimateCooldownModifiers.Contains(modifierToRemove)) return;
        
        ultimateCooldownModifiers.Remove(modifierToRemove);
        _bonusUltimateCooldown /= (1 + modifierToRemove.bonusUltimateCooldown);
        
        CooldownDuration = UltimateAbilityData.cooldown * _bonusUltimateCooldown;
    }

    public virtual void AddUltimateDamageModifier(UltimateDamageModifier modifierToAdd)
    {
        if (ultimateDamageModifiers.Contains(modifierToAdd)) return;
        
        ultimateDamageModifiers.Add(modifierToAdd);
        BonusUltimateDamage += BonusUltimateDamage * modifierToAdd.bonusUltimateDamage;
    }

    public virtual void RemoveUltimateDamageModifier(UltimateDamageModifier modifierToRemove)
    {
        if (!ultimateDamageModifiers.Contains(modifierToRemove)) return;
        
        ultimateDamageModifiers.Remove(modifierToRemove);
        BonusUltimateDamage /= (1 + modifierToRemove.bonusUltimateDamage);
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

    public void RegisterAllAddModifierMethods()
    {
        ModifierManager.RegisterAddMethod<UltimateCooldownModifier>(AddUltimateCooldownModifier);
        ModifierManager.RegisterAddMethod<UltimateDamageModifier>(AddUltimateDamageModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        ModifierManager.RegisterRemoveMethod<UltimateCooldownModifier>(RemoveUltimateCooldownModifier);
        ModifierManager.RegisterRemoveMethod<UltimateDamageModifier>(RemoveUltimateDamageModifier);
    }
}

