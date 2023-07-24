using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public abstract class UltimateAbility<T> : MonoBehaviour, IUltimateUpdatable where T: UltimateAbilityData
{
    [SerializeField] protected PlayerEvents playerEvents;
    
    [SerializeField] protected T ultimateData;
    
    private TopDownInput _topDownInput;

    private InputAction _ultimateInputAction;
    
    private bool _canUseUltimate;

    private void Awake()
    {
        _topDownInput = new TopDownInput();
        _ultimateInputAction = _topDownInput.Player.Ultimate;
    }

    protected virtual void OnEnable()
    {
        _topDownInput.Enable();

        _ultimateInputAction.performed += ActivateUltimate;
        StartCooldowns();
    }
    
    protected virtual void OnDisable()
    {
        _topDownInput.Disable();
        _ultimateInputAction.performed -= ActivateUltimate;
        StopAllCoroutines();
    }
    
    protected virtual void Start()
    {
        _canUseUltimate = false;
        
        playerEvents.InvokeUltimateNameUpdatedEvent(ultimateData.abilityName);
    }

    ///-///////////////////////////////////////////////////////////
    /// When player presses 'Q' or ultimate button, 
    /// activate the ultimate ability associated with the character
    public void ActivateUltimate(CallbackContext context)
    {
        if (_canUseUltimate)
        {
            Debug.Log("Activate ultimate!");

            UltimateAction(gameObject);
                
            _canUseUltimate = false;
                
        }
    }
    protected abstract void UltimateAction(GameObject player);
    
    protected void StartCooldowns()
    {
        StartCoroutine(UltimateCooldown());
        StartCoroutine(CountDownCooldown());
    }

    ///-///////////////////////////////////////////////////////////
    /// Wait some time to allow player to use ultimate ability again
    /// 
    private IEnumerator UltimateCooldown()
    {
        yield return new WaitForSeconds(ultimateData.cooldown);

        _canUseUltimate = true;
        
        Debug.Log("Ultimate is ready.");
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Show on the HUD, the current ultimate ability's cooldown
    /// counting down. Once it hits 0, the player can use their ultimate.
    /// 
    private IEnumerator CountDownCooldown()
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

    public void AllowUltimate(bool boolean)
    {
        if (boolean)
        {
            _ultimateInputAction.Enable();
        }
        else
        {
            _ultimateInputAction.Disable();
        }
    }
    public void UpdateScriptableObject(UltimateAbilityData scriptableObject)
    {
        if (scriptableObject is T)
        {
            ultimateData = scriptableObject as T;
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }
}

