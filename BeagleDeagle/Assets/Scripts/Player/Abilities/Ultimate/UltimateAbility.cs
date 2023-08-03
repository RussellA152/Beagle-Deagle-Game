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
        StartCooldown();
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
    
    protected void StartCooldown()
    {
        StartCoroutine(CountDownCooldown());
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Show on the HUD, the current ultimate ability's cooldown
    /// counting down. Once it hits 0, the player can use their ultimate.
    /// 
    private IEnumerator CountDownCooldown()
    {
        float remainingTime = ultimateData.cooldown;
        playerEvents.InvokeUltimateAbilityCooldownEvent(remainingTime);

        _canUseUltimate = false;
        
        // Decrement the cooldown by 1 second
        // Invoke event system that takes in the amount of time left on the ultimate's cooldown (displays on the HUD)
        while (remainingTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
            playerEvents.InvokeUltimateAbilityCooldownEvent(remainingTime);
        }
        _canUseUltimate = true;
        playerEvents.InvokeUltimateAbilityCooldownEvent(remainingTime);
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

