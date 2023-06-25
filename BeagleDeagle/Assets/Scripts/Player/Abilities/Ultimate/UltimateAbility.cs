using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class UltimateAbility : MonoBehaviour
{
    [SerializeField]
    private PlayerEventSO playerEvents;

    [SerializeField]
    private UltimateAbilityData ultimateData;

    private bool canUseUltimate;

    private void OnEnable()
    {
        StartCoroutine(WaitForUse());
        StartCoroutine(CountDownCooldown());
    }

    private void Start()
    {
        playerEvents.InvokeUltimateNameUpdatedEvent(ultimateData.name);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void ActivateUltimate(CallbackContext inputValue)
    {
        if (inputValue.performed)
        {
            if (canUseUltimate)
            {
                Debug.Log("Activate ultimate!");

                canUseUltimate = false;

                ultimateData.ActivateUltimate(gameObject);

                StartCoroutine(WaitForUse());

                StartCoroutine(CountDownCooldown());

            }
        }
    }

    public IEnumerator WaitForUse()
    {
        canUseUltimate = false;

        yield return StartCoroutine(ultimateData.ActivationCooldown());

        canUseUltimate = true;

        Debug.Log("Can use ultimate now!");
    }

    public IEnumerator CountDownCooldown()
    {
        // Wait until ultimate is no longer active to begin cooldown
        while (ultimateData.isActive)
            yield return null;

        float timeLeft = ultimateData.cooldown; // Ex. 5 seconds until cooldown

        // If the cooldown is 0 seconds or less, don't continue further
        if (timeLeft <= 0f)
            yield break;

        playerEvents.InvokeUltimateAbilityCooldownEvent(timeLeft);

        // Decrement the cooldown by 1 second
        // Invoke event system that takes in the amount of time left on the ultimate's cooldown
        while (timeLeft > 0f)
        {
            yield return new WaitForSeconds(1f);

            timeLeft--;

            playerEvents.InvokeUltimateAbilityCooldownEvent(timeLeft);
        }

        playerEvents.InvokeUltimateAbilityCooldownEvent(0f);
    }
}
