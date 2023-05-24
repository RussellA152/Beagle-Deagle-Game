using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Abilities : MonoBehaviour
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    private IPlayerStatModifier playerStatModifierScript;

    // a list of the player's passive abilities
    [SerializeField]
    private List<PassiveAbilityData> passives = new List<PassiveAbilityData>();

    [SerializeField]
    private UtilityAbilityData utility;

    private int utilityUses;

    private void Awake()
    {
        playerEvents.givePlayerStatModifierScriptEvent += UpdatePlayerStatsModifierScript;
    }

    private void OnEnable()
    {
        utilityUses = utility.maxUses;
        
    }

    private void Start()
    {
        playerEvents.InvokeUtilityUsesUpdatedEvent(utilityUses + playerStatModifierScript.GetUtilityUsesModifier());
        playerEvents.InvokeUtilityNameUpdatedEvent(utility.name);
    }

    private void OnDestroy()
    {
        playerEvents.givePlayerStatModifierScriptEvent -= UpdatePlayerStatsModifierScript;
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
            if ((utilityUses + playerStatModifierScript.GetUtilityUsesModifier()) > 0)
            {
                Debug.Log("Activate utility!");

                utilityUses--;

                utility.ActivateUtility(gameObject);

                playerEvents.InvokeUtilityUsesUpdatedEvent(utilityUses + playerStatModifierScript.GetUtilityUsesModifier());

                StartCoroutine(StartUtilityCooldown());
            }
        }     
        
    }

    public void UpdatePlayerStatsModifierScript(IPlayerStatModifier modifierScript)
    {
        playerStatModifierScript = modifierScript;
    }

    IEnumerator StartUtilityCooldown()
    {
        // Start the cooldown that comes from the Utility scriptable object.
        // We start a coroutine within another coroutine so that we don't have to modify the
        // uses variable within the Utility scriptable object
        yield return new WaitForSeconds(utility.cooldown * playerStatModifierScript.GetUtilityCooldownModifier());

        utilityUses++;

        playerEvents.InvokeUtilityUsesUpdatedEvent(utilityUses + playerStatModifierScript.GetUtilityUsesModifier());

    }
}

