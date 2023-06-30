using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class UltimateAbility<T> : MonoBehaviour where T: UltimateAbilityData
{
    [SerializeField]
    protected PlayerEvents playerEvents;
    
    [SerializeField]
    protected T ultimateData;

    [SerializeField]
    protected bool canUseUltimate;
    

    protected virtual void OnEnable()
    {
        StartCoroutine(WaitForUse());
        StartCoroutine(CountDownCooldown());
    }

    protected virtual void Start()
    {
        playerEvents.InvokeUltimateNameUpdatedEvent(ultimateData.name);
    }

    protected virtual void OnDisable()
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

                UltimateAction(gameObject);
                
                //StartCoroutine(WaitForUse());
                //StartCoroutine(CountDownCooldown());

            }
        }
    }

    protected abstract void UltimateAction(GameObject player);


    protected virtual IEnumerator WaitForUse()
    {
        canUseUltimate = false;
        
        yield return new WaitForSeconds(ultimateData.cooldown);

        canUseUltimate = true;
        

        Debug.Log("Can use ultimate now!");
    }

    protected virtual IEnumerator CountDownCooldown()
    {
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

