using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ModifierRemover : MonoBehaviour
{
    // Dictionary to store the remove methods for each type
    private Dictionary<Type, Action<object>> _removeMethods = new Dictionary<Type, Action<object>>();
    

    // Register a remove method for a specific type
    public void RegisterRemoveMethod<T>(Action<T> removeMethod) where T : Modifier
    {
        Type type = typeof(T);
        _removeMethods[type] = obj => removeMethod.Invoke((T)obj);
    }

    public void RemoveModifierAfterDelay<T>(T modifierToRemove, float delay) where T : Modifier
    {
        StartCoroutine(RemoveDelayCoroutine(modifierToRemove, delay));
    }

    // Coroutine to remove a modifier after a delay
    private IEnumerator RemoveDelayCoroutine<T>(T modifierToRemove, float delay) where T : Modifier
    {

        yield return new WaitForSeconds(delay);

        // Find and invoke the remove method for the type T
        Type type = typeof(T);
        if (_removeMethods.TryGetValue(type, out Action<object> removeMethod))
        {
            removeMethod.Invoke(modifierToRemove);
        }
    }
    
}
