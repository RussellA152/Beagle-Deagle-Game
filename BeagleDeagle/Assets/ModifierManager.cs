using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

///-///////////////////////////////////////////////////////////
/// Responsible for storing remove methods for any type of modifier and calling them after 'x' seconds.
/// Ex. Removing an AttackSpeedModifier after 3 seconds after an enemy exits a slow field.
public class ModifierManager : MonoBehaviour
{
    // Dictionary to store the add methods for each type
    private Dictionary<Type, Action<object>> _addMethods = new Dictionary<Type, Action<object>>(); 
    
    // Dictionary to store the remove methods for each type
    private Dictionary<Type, Action<object>> _removeMethods = new Dictionary<Type, Action<object>>();
    
    public void RegisterAddMethod<T>(Action<T> addMethod) where T : Modifier
    {
        Type type = typeof(T);
        _addMethods[type] = obj => addMethod.Invoke((T)obj);
    }

    public void AddModifier<T>(T modifierToAdd) where T : Modifier
    {
        Type type = typeof(T);
        
        if (_addMethods.TryGetValue(type, out Action<object> removeMethod))
        {
            removeMethod.Invoke(modifierToAdd);
        }
        else
        {
            Debug.LogError($"{gameObject.name} does not have {type} modifier that they can add. Or they haven't registered the add method.");
        }
    }

    // Register a remove method for a specific type
    public void RegisterRemoveMethod<T>(Action<T> removeMethod) where T : Modifier
    {
        Type type = typeof(T);
        _removeMethods[type] = obj => removeMethod.Invoke((T)obj);
    }
    
    public void RemoveModifier<T>(T modifierToRemove) where T : Modifier
    {
        // Find and invoke the remove method for the type T
        Type type = typeof(T);
        
        if (_removeMethods.TryGetValue(type, out Action<object> removeMethod))
        {
            removeMethod.Invoke(modifierToRemove);
        }
        else
        {
            Debug.LogError($"{gameObject.name} does not have {type} modifier that they can remove. Or they haven't registered the remove method.");
        }
    }

    public void RemoveModifierAfterDelay<T>(T modifierToRemove, float delay) where T : Modifier
    {
        StartCoroutine(RemoveDelayCoroutine(modifierToRemove, delay));
    }

    // Coroutine to remove a modifier after a delay
    private IEnumerator RemoveDelayCoroutine<T>(T modifierToRemove, float delay) where T : Modifier
    {
        yield return new WaitForSeconds(delay);

        RemoveModifier(modifierToRemove);
    }
    
}
