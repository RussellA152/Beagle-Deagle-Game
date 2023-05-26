using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Abilities : MonoBehaviour
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    // a list of the player's passive abilities
    [SerializeField]
    private List<PassiveAbilityData> passives = new List<PassiveAbilityData>();

    [SerializeField]
    private UtilityAbilityData utility;

    [SerializeField, NonReorderable]
    private List<UtilityCooldownModifier> utilityCooldownModifiers = new List<UtilityCooldownModifier>();

    [SerializeField, NonReorderable]
    private List<UtilityUsesModifier> utilityUsesModifiers = new List<UtilityUsesModifier>();

    private int utilityUses;
    private int bonusUtilityUses;

    private float bonusUtilityCooldown;

    private void OnEnable()
    {
        utilityUses = utility.maxUses;
        
    }

    private void Start()
    {
        ActivateAllPassives();
        UtilityUsesModified();
        playerEvents.InvokeUtilityNameUpdatedEvent(utility.name);
    }

    public void ActivateAllPassives()
    {
        foreach (PassiveAbilityData passive in passives)
        {
            if (passive.activationType == PassiveAbilityData.PassiveActivationType.Once)
                passive.ActivatePassive(gameObject);

            else if (passive.activationType == PassiveAbilityData.PassiveActivationType.Continuous)
                StartCoroutine(StartContinuousPassive(passive));
        }
    }
    // Continuous passives will activate many times, so we use a coroutine
    private IEnumerator StartContinuousPassive(PassiveAbilityData passive)
    {
        while (true)
        {
            passive.ActivatePassive(gameObject);
            yield return null;
        }
    }

    public void ActivateUtility(CallbackContext inputValue)
    {
        if (inputValue.performed)
        {
            // If player has uses left on their utility ability, let them activate it 
            // We also take into account any items that upgraded the number of uses on their utility ability
            if ((utilityUses + bonusUtilityUses) > 0)
            {
                Debug.Log("Activate utility!");

                utilityUses--;

                utility.ActivateUtility(gameObject);

                UtilityUsesModified();

                StartCoroutine(StartUtilityCooldown());
            }
        }     
        
    }

    public void UtilityUsesModified()
    {
        playerEvents.InvokeUtilityUsesUpdatedEvent(utilityUses + bonusUtilityUses);
    }

    IEnumerator StartUtilityCooldown()
    {
        // Start the cooldown that comes from the Utility scriptable object.
        // We start a coroutine within another coroutine so that we don't have to modify the
        // uses variable within the Utility scriptable object
        yield return new WaitForSeconds(utility.cooldown * bonusUtilityCooldown);

        utilityUses++;

        UtilityUsesModified();

    }

    public void RegisterUtilityCooldownModifier(UtilityCooldownModifier modifierToAdd)
    {
        utilityCooldownModifiers.Add(modifierToAdd);
        bonusUtilityCooldown += modifierToAdd.bonusUtilityCooldown;
    }

    public void DeregisterUtilityCooldownModifier(UtilityCooldownModifier modifierToRemove)
    {
        utilityCooldownModifiers.Remove(modifierToRemove);
        bonusUtilityCooldown -= modifierToRemove.bonusUtilityCooldown;
    }

    public void RegisterUtilityUsesModifier(UtilityUsesModifier modifierToAdd)
    {
        utilityUsesModifiers.Add(modifierToAdd);
        bonusUtilityUses += modifierToAdd.bonusUtilityUses;
    }

    public void DeregisterUtilityUsesModifier(UtilityUsesModifier modifierToRemove)
    {
        utilityUsesModifiers.Remove(modifierToRemove);
        bonusUtilityUses -= modifierToRemove.bonusUtilityUses;
    }
}

