using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class UtilityAbility : MonoBehaviour
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    [SerializeField]
    private UtilityAbilityData utilityData;

    private bool canUseUtility = true;

    private float delayBetweenUse = 0.4f; // a small delay added between each utility use (prevents player from using too many at once)

    [SerializeField, NonReorderable]
    private List<UtilityCooldownModifier> utilityCooldownModifiers = new List<UtilityCooldownModifier>();

    [SerializeField, NonReorderable]
    private List<UtilityUsesModifier> utilityUsesModifiers = new List<UtilityUsesModifier>();

    private int utilityUses;
    private int bonusUtilityUses = 0;

    private float bonusUtilityCooldown = 1f;

    private void OnEnable()
    {
        utilityUses = utilityData.maxUses;
    }

    private void Start()
    {
        UtilityUsesModified();
        playerEvents.InvokeUtilityNameUpdatedEvent(utilityData.name);
    }

    public void ActivateUtility(CallbackContext inputValue)
    {
        if (inputValue.performed)
        {
            // If player has uses left on their utility ability, let them activate it 
            // We also take into account any items that upgraded the number of uses on their utility ability
            if (canUseUtility && ((utilityUses + bonusUtilityUses) > 0))
            {
                Debug.Log("Activate utility!");

                utilityUses--;

                // Pass in the object pool (to spawn objects like grenades and bullets), and the player gameobject
                utilityData.ActivateUtility(ObjectPooler.instance, gameObject);

                UtilityUsesModified();

                StartCoroutine(StartUtilityCooldown());

                StartCoroutine(StartDelayBetweenUtilityUse());
            }
        }

    }
    public void UtilityUsesModified()
    {
        playerEvents.InvokeUtilityUsesUpdatedEvent(utilityUses + bonusUtilityUses);
    }

    // This coroutine will add a small delay between each use of a utility
    IEnumerator StartDelayBetweenUtilityUse()
    {
        canUseUtility = false;

        yield return new WaitForSeconds(delayBetweenUse);

        canUseUtility = true;
    }

    // Start the cooldown that comes from the Utility scriptable object.
    // We start a coroutine within another coroutine so that we don't have to modify the...
    // uses variable within the Utility scriptable object.
    IEnumerator StartUtilityCooldown()
    {
        yield return new WaitForSeconds(utilityData.cooldown * bonusUtilityCooldown);

        utilityUses++;

        UtilityUsesModified();

    }
    public void AddUtilityCooldownModifier(UtilityCooldownModifier modifierToAdd)
    {
        utilityCooldownModifiers.Add(modifierToAdd);
        bonusUtilityCooldown += (bonusUtilityCooldown * modifierToAdd.bonusUtilityCooldown);
    }

    public void RemoveUtilityCooldownModifier(UtilityCooldownModifier modifierToRemove)
    {
        utilityCooldownModifiers.Remove(modifierToRemove);
        bonusUtilityCooldown /= (1 + modifierToRemove.bonusUtilityCooldown);
    }

    public void AddUtilityUsesModifier(UtilityUsesModifier modifierToAdd)
    {
        utilityUsesModifiers.Add(modifierToAdd);
        bonusUtilityUses += modifierToAdd.bonusUtilityUses;
    }

    public void RemoveUtilityUsesModifier(UtilityUsesModifier modifierToRemove)
    {
        utilityUsesModifiers.Remove(modifierToRemove);
        bonusUtilityUses -= modifierToRemove.bonusUtilityUses;
    }
}
