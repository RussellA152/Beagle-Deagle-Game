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
    
    [SerializeField] protected T utilityData;

    public CooldownSystem CooldownSystem;

    private PlayerInput _playerInput;
    //private TopDownInput _topDownInput;

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
        CooldownSystem = GetComponent<CooldownSystem>();

        _utilityInputAction = _playerInput.currentActionMap.FindAction("Utility");

        Id = 12;
        
        CooldownDuration = utilityData.cooldown;
    }

    private void OnEnable()
    {
        _utilityInputAction.performed += ActivateUtility;
        _utilityUses = utilityData.maxUses;

        CooldownSystem.OnCooldownEnded += UtilityUsesModified;
    }

    private void OnDisable()
    {
        _utilityInputAction.performed -= ActivateUtility;
        CooldownSystem.OnCooldownEnded -= UtilityUsesModified;
    }
    
    protected virtual void Start()
    {
        playerEvents.InvokeUtilityCooldown(Id);
        playerEvents.InvokeNewUtility(utilityData);
        playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);
    }
    
    public void ActivateUtility(CallbackContext context)
    {
        // If player has uses left on their utility ability, let them activate it 
        // We also take into account any items that upgraded the number of uses on their utility ability
        if (_canUseUtility && _onDelayCooldown && ((_utilityUses + _bonusUtilityUses) > 0))
        {
            _utilityUses--;
            
            Debug.Log(CooldownSystem.GetRemainingDuration(Id));
                
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
            playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);
        }
        
    }

    ///-///////////////////////////////////////////////////////////
    /// Start a cooldown for a utility. Only one cooldown is occuring at a time.
    /// 
    private IEnumerator WaitCooldown()
    {
        while (CooldownSystem.IsOnCooldown(Id))
        {
            yield return null;
        }
        CooldownSystem.PutOnCooldown(this);
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

    public void AllowUtility(bool boolean)
    {
        _canUseUtility = boolean;
    }
    
    public void UpdateScriptableObject(UtilityAbilityData scriptableObject)
    {
        if (scriptableObject is T)
        {
            utilityData = scriptableObject as T;
            playerEvents.InvokeNewUtility(utilityData);
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

        modifierToAdd.isActive = true;
    }

    public void RemoveUtilityCooldownModifier(UtilityCooldownModifier modifierToRemove)
    {
        utilityCooldownModifiers.Remove(modifierToRemove);
        _bonusUtilityCooldown /= (1 + modifierToRemove.bonusUtilityCooldown);

        modifierToRemove.isActive = false;
    }

    public void AddUtilityUsesModifier(UtilityUsesModifier modifierToAdd)
    {
        utilityUsesModifiers.Add(modifierToAdd);
        _bonusUtilityUses += modifierToAdd.bonusUtilityUses;
        
        modifierToAdd.isActive = true;
    }

    public void RemoveUtilityUsesModifier(UtilityUsesModifier modifierToRemove)
    {
        utilityUsesModifiers.Remove(modifierToRemove);
        _bonusUtilityUses -= modifierToRemove.bonusUtilityUses;

        modifierToRemove.isActive = false;
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
