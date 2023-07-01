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
    
    private bool _canUseUltimate;
    

    protected virtual void OnEnable()
    {
        StartCoroutine(UltimateCooldown());
        StartCoroutine(CountDownCooldown());
    }

    protected virtual void Start()
    {
        _canUseUltimate = false;
        
        playerEvents.InvokeUltimateNameUpdatedEvent(ultimateData.name);
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    ///-///////////////////////////////////////////////////////////
    /// When player presses 'Q' or ultimate button, 
    /// activate the ultimate ability associated with the character
    public void ActivateUltimate(CallbackContext inputValue)
    {
        if (inputValue.performed)
        {
            if (_canUseUltimate)
            {
                Debug.Log("Activate ultimate!");

                UltimateAction(gameObject);
                
                _canUseUltimate = false;
                
            }
        }
    }

    protected abstract void UltimateAction(GameObject player);

    ///-///////////////////////////////////////////////////////////
    /// Wait some time to allow player to use ultimate ability again
    /// 
    protected IEnumerator UltimateCooldown()
    {
        yield return new WaitForSeconds(ultimateData.cooldown);

        _canUseUltimate = true;
        
        Debug.Log("Ultimate is ready.");
    }

    

    protected IEnumerator CountDownCooldown()
    {
        // Ex. 5 seconds until cooldown
        float timeLeft = ultimateData.cooldown;
    
        // If the cooldown is 0 seconds or less, don't continue further
        if (timeLeft <= 0f)
            yield break;
    
        playerEvents.InvokeUltimateAbilityCooldownEvent(timeLeft);
    
        // Decrement the cooldown by 1 second
        // Invoke event system that takes in the amount of time left on the ultimate's cooldown (displays on the HUD)
        while (timeLeft > 0f)
        {
            yield return new WaitForSeconds(1f);
    
            timeLeft--;
    
            playerEvents.InvokeUltimateAbilityCooldownEvent(timeLeft);
        }
    
        playerEvents.InvokeUltimateAbilityCooldownEvent(0f);
    }
}

