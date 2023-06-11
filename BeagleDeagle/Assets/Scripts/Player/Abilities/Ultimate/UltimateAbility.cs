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
        Debug.Log(ultimateData.name);

        StartCoroutine(WaitForUse());
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

}
