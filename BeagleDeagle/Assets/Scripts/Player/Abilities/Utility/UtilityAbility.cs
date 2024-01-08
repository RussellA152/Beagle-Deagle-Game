using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;

public abstract class UtilityAbility<T> : MonoBehaviour, IUtilityUpdatable, IHasCooldown, IHasInput where T: UtilityAbilityData
{
    [SerializeField] private PlayerEvents playerEvents;
    private AudioClipPlayer _audioClipPlayer;

    [SerializeField] private PlayerData playerData;
    protected T UtilityData;

    private CooldownSystem _cooldownSystem;
    protected MiscellaneousModifierList MiscellaneousModifierList;

    private PlayerInput _playerInput;

    private InputAction _utilityInputAction;

    private bool _canUseUtility = true;

    // A small delay added between each utility use (prevents player from using too many at once)
    private float _delayBetweenUse = 0.6f; 

    // Don't allow player to use utility if the use delay is active
    private bool _onDelayCooldown = true;
    
    [Header("Modifiers")]
    [SerializeField, NonReorderable] private List<UtilityCooldownModifier> utilityCooldownModifiers = new List<UtilityCooldownModifier>();
    [SerializeField, NonReorderable] private List<UtilityUsesModifier> utilityUsesModifiers = new List<UtilityUsesModifier>();
    

    private int _utilityUses;
    private int _bonusUtilityUses = 0;

    private float _bonusUtilityCooldown = 1f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _cooldownSystem = GetComponent<CooldownSystem>();
        _audioClipPlayer = GetComponent<AudioClipPlayer>();
        MiscellaneousModifierList = GetComponent<MiscellaneousModifierList>();

        _utilityInputAction = _playerInput.currentActionMap.FindAction("Utility");
        
        UtilityData = (T) playerData.utilityAbilityData;
        
        
    }

    private void OnEnable()
    {
        _utilityInputAction.performed += ActivateUtility;
        _utilityUses = UtilityData.maxUses;

        _cooldownSystem.OnCooldownEnded += UtilityUsesModified;
    }

    private void OnDisable()
    {
        _utilityInputAction.performed -= ActivateUtility;
        _cooldownSystem.OnCooldownEnded -= UtilityUsesModified;
    }
    
    protected virtual void Start()
    {
        Id = _cooldownSystem.GetAssignableId();
        CooldownDuration = UtilityData.cooldown * _bonusUtilityCooldown;
        
        playerEvents.InvokeUtilityCooldown(Id);
        playerEvents.InvokeNewUtility(UtilityData);
        playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);
    }
    
    public void ActivateUtility(CallbackContext context)
    {
        // If player has uses left on their utility ability, let them activate it 
        // We also take into account any items that upgraded the number of uses on their utility ability
        if (_canUseUtility && _onDelayCooldown && ((_utilityUses + _bonusUtilityUses) > 0))
        {
            _utilityUses--;
            
            PlayActivationSound();
                
            UtilityAction();
            
            playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);
            
            StartCoroutine(WaitCooldown());
            StartCoroutine(StartDelayBetweenUtilityUse());
        }
    }

    protected abstract void UtilityAction();

    private void UtilityUsesModified(int id)
    {
        if (id == Id)
        {
            _utilityUses++;
            // Make sure utility uses doesn't go past maximum allowed
            _utilityUses = Mathf.Clamp(_utilityUses, 0, UtilityData.maxUses);
            
            playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);
        }
        
    }

    ///-///////////////////////////////////////////////////////////
    /// Start a cooldown for a utility. Only one cooldown is occuring at a time.
    /// 
    private IEnumerator WaitCooldown()
    {
        while (_cooldownSystem.IsOnCooldown(Id))
        {
            yield return null;
        }
        _cooldownSystem.PutOnCooldown(this);
    }
    

    ///-///////////////////////////////////////////////////////////
    /// Small delay between each use of a utility to prevent spamming.
    /// 
    private IEnumerator StartDelayBetweenUtilityUse()
    {
        _onDelayCooldown = false;
    
        yield return new WaitForSeconds(_delayBetweenUse);
    
        _onDelayCooldown = true;
    }

    ///-///////////////////////////////////////////////////////////
    /// Refill all uses and stop any ongoing usage cooldown.
    /// 
    private void RefillAllUsages()
    {
        _utilityUses = UtilityData.maxUses;
        playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);
        _cooldownSystem.RemoveCooldown(Id);
    }

    private void PlayActivationSound()
    {
        _audioClipPlayer.PlayGeneralAudioClip(UtilityData.activationSound, UtilityData.activationSoundVolume);
    }

    public void AllowUtility(bool boolean)
    {
        _canUseUtility = boolean;
    }
    
    public virtual void UpdateScriptableObject(UtilityAbilityData scriptableObject)
    {
        if (scriptableObject is T)
        {
            UtilityData = scriptableObject as T;
            playerEvents.InvokeNewUtility(UtilityData);
            
            RefillAllUsages();
            
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }

    #region UtilityModifiers

    public void AddUtilityCooldownModifier(UtilityCooldownModifier modifierToAdd)
    {
        utilityCooldownModifiers.Add(modifierToAdd);
        _bonusUtilityCooldown += (_bonusUtilityCooldown * modifierToAdd.bonusUtilityCooldown);
        
        CooldownDuration = UtilityData.cooldown * _bonusUtilityCooldown;

    }

    public void RemoveUtilityCooldownModifier(UtilityCooldownModifier modifierToRemove)
    {
        utilityCooldownModifiers.Remove(modifierToRemove);
        _bonusUtilityCooldown /= (1 + modifierToRemove.bonusUtilityCooldown);
        
        CooldownDuration = UtilityData.cooldown * _bonusUtilityCooldown;
    }

    public void AddUtilityUsesModifier(UtilityUsesModifier modifierToAdd)
    {
        utilityUsesModifiers.Add(modifierToAdd);
        _bonusUtilityUses += modifierToAdd.bonusUtilityUses;
        
        playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);

    }

    public void RemoveUtilityUsesModifier(UtilityUsesModifier modifierToRemove)
    {
        utilityUsesModifiers.Remove(modifierToRemove);
        _bonusUtilityUses -= modifierToRemove.bonusUtilityUses;
        
        playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);

    }

    #endregion
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
    public void AllowInput(bool boolean)
    {
        if (boolean)
        {
            _utilityInputAction.Enable();
        }
        else
        {
            _utilityInputAction.Disable();
        }
    }
}
