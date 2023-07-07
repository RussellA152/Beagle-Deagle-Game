using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class UtilityActivator : MonoBehaviour
{
    [SerializeField]
    private PlayerEvents playerEvents;

    [SerializeField]
    private UtilityAbilityData currentUtilityData;

    private bool _canUseUtility = true;

    private float _delayBetweenUse = 0.4f; // a small delay added between each utility use (prevents player from using too many at once)

    [SerializeField, NonReorderable]
    private List<UtilityCooldownModifier> utilityCooldownModifiers = new List<UtilityCooldownModifier>();

    [SerializeField, NonReorderable]
    private List<UtilityUsesModifier> utilityUsesModifiers = new List<UtilityUsesModifier>();

    [SerializeField] 
    private UnityEvent<GameObject> onUtilityUse;

    private int _utilityUses;
    private int _bonusUtilityUses = 0;

    private float _bonusUtilityCooldown = 1f;
    
    private void OnEnable()
    {
        _utilityUses = currentUtilityData.maxUses;
    }

    protected virtual void Start()
    {
        UtilityUsesModified();
        playerEvents.InvokeUtilityNameUpdatedEvent(currentUtilityData.abilityName);
    }

    public void ActivateUtility(CallbackContext inputValue)
    {
        if (inputValue.performed)
        {
            // If player has uses left on their utility ability, let them activate it 
            // We also take into account any items that upgraded the number of uses on their utility ability
            if (_canUseUtility && ((_utilityUses + _bonusUtilityUses) > 0))
            {
                Debug.Log("Activate utility!");

                _utilityUses--;
                
                onUtilityUse.Invoke(gameObject);

                UtilityUsesModified();

                StartCoroutine(StartUtilityCooldown());

                StartCoroutine(StartDelayBetweenUtilityUse());
            }
        }

    }

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
