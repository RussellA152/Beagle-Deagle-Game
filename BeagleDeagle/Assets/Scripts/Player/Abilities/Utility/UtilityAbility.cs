using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class UtilityAbility<T> : MonoBehaviour, IUtilityUpdatable, IHasCooldown where T: UtilityAbilityData
{
    [SerializeField] private PlayerEvents playerEvents;
    
    [SerializeField] protected T currentUtilityData;

    public CooldownSystem CooldownSystem;

    private PlayerInput _playerInput;
    //private TopDownInput _topDownInput;

    private InputAction _utilityInputAction;

    private bool _canUseUtility = true;

    private float _delayBetweenUse = 0.6f; // a small delay added between each utility use (prevents player from using too many at once)

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
        
        CooldownDuration = currentUtilityData.cooldown;
    }

    private void OnEnable()
    {
        _utilityInputAction.performed += ActivateUtility;
        _utilityUses = currentUtilityData.maxUses;

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
        playerEvents.InvokeNewUtility(currentUtilityData);
        playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);
    }
    
    public void ActivateUtility(CallbackContext context)
    {
        // If player has uses left on their utility ability, let them activate it 
        // We also take into account any items that upgraded the number of uses on their utility ability
        if (_canUseUtility && ((_utilityUses + _bonusUtilityUses) > 0))
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
        _canUseUtility = false;
    
        yield return new WaitForSeconds(_delayBetweenUse);
    
        _canUseUtility = true;
    }

    public void AllowUtility(bool boolean)
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
    
    public void UpdateScriptableObject(UtilityAbilityData scriptableObject)
    {
        if (scriptableObject is T)
        {
            currentUtilityData = scriptableObject as T;
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
    }

    public void RemoveUtilityCooldownModifier(UtilityCooldownModifier modifierToRemove)
    {
        utilityCooldownModifiers.Remove(modifierToRemove);
        _bonusUtilityCooldown /= (1 + modifierToRemove.bonusUtilityCooldown);
    }

    public void AddUtilityUsesModifier(UtilityUsesModifier modifierToAdd)
    {
        utilityUsesModifiers.Add(modifierToAdd);
        _bonusUtilityUses += modifierToAdd.bonusUtilityUses;
    }

    public void RemoveUtilityUsesModifier(UtilityUsesModifier modifierToRemove)
    {
        utilityUsesModifiers.Remove(modifierToRemove);
        _bonusUtilityUses -= modifierToRemove.bonusUtilityUses;
    }

    #endregion
    
    public int Id { get; set; }
    public float CooldownDuration { get; set; }
}
