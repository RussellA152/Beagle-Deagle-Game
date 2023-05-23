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

    private int utilityUses;

    private void Start()
    {
        utilityUses = utility.uses;
        playerEvents.InvokeUtilityUsesUpdatedEvent(utilityUses);
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
            if (utilityUses > 0)
            {
                Debug.Log("Activate utility!");

                utilityUses--;

                utility.ActivateUtility(gameObject);

                playerEvents.InvokeUtilityUsesUpdatedEvent(utilityUses);

                StartCoroutine(StartUtilityCooldown());
            }
        }
        
        
        
    }

    IEnumerator StartUtilityCooldown()
    {
        // Start the cooldown that comes from the Utility scriptable object.
        // We start a coroutine within another coroutine so that we don't have to modify the
        // uses variable within the Utility scriptable object
        yield return new WaitForSeconds(utility.cooldown);

        utilityUses++;

        playerEvents.InvokeUtilityUsesUpdatedEvent(utilityUses);

    }
}

