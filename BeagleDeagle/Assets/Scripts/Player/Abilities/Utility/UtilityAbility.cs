using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class UtilityAbility<T> : MonoBehaviour, IUtilityUpdatable where T: UtilityAbilityData
{
    [SerializeField] private PlayerEvents playerEvents;
    
    [SerializeField] protected T currentUtilityData;

    private TopDownInput _topDownInput;

    private InputAction _utilityInputAction;

    private bool _canUseUtility = true;

    private float _delayBetweenUse = 0.4f; // a small delay added between each utility use (prevents player from using too many at once)

    [Header("Modifiers")]
    [SerializeField, NonReorderable] private List<UtilityCooldownModifier> utilityCooldownModifiers = new List<UtilityCooldownModifier>();
    [SerializeField, NonReorderable] private List<UtilityUsesModifier> utilityUsesModifiers = new List<UtilityUsesModifier>();
    

    private int _utilityUses;
    private int _bonusUtilityUses = 0;

    private float _bonusUtilityCooldown = 1f;

    private void Awake()
    {
        _topDownInput = new TopDownInput();
        _utilityInputAction = _topDownInput.Player.Utility;
    }

    private void OnEnable()
    {
        _topDownInput.Enable();
        _utilityInputAction.performed += ActivateUtility;
        _utilityUses = currentUtilityData.maxUses;
    }

    private void OnDisable()
    {
        _topDownInput.Disable();
        _utilityInputAction.performed -= ActivateUtility;
    }
    
    protected virtual void Start()
    {
        UtilityUsesModified();
        playerEvents.InvokeUtilityNameUpdatedEvent(currentUtilityData.abilityName);
    }

    public void ActivateUtility(CallbackContext context)
    {
        // If player has uses left on their utility ability, let them activate it 
        // We also take into account any items that upgraded the number of uses on their utility ability
        if (_canUseUtility && ((_utilityUses + _bonusUtilityUses) > 0))
        {
            Debug.Log("Activate utility!");

            _utilityUses--;
                
            //onUtilityUse.Invoke(gameObject);
                
            UtilityAction(gameObject);

            UtilityUsesModified();

            StartCoroutine(StartUtilityCooldown());

            StartCoroutine(StartDelayBetweenUtilityUse());
        }

    }

    protected abstract void UtilityAction(GameObject player);

    private void UtilityUsesModified()
    {
        playerEvents.InvokeUtilityUsesUpdatedEvent(_utilityUses + _bonusUtilityUses);
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

    ///-///////////////////////////////////////////////////////////
    /// Wait some time to add another use to the utility.
    /// Start the cooldown that comes from the Utility scriptable object.
    /// 
    private IEnumerator StartUtilityCooldown()
    {
        yield return new WaitForSeconds(currentUtilityData.cooldown * _bonusUtilityCooldown);

        _utilityUses++;

        UtilityUsesModified();

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
    
}
