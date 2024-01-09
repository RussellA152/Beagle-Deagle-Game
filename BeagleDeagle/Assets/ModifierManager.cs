using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

///-///////////////////////////////////////////////////////////
/// Responsible for storing add and remove modifier from a gameObject methods for any type of modifier and calling them after 'x' seconds.
/// Ex. Removing an AttackSpeedModifier after 3 seconds after an enemy exits a slow field.
public class ModifierManager : MonoBehaviour
{
    // Dictionary to store the add modifier methods
    private Dictionary<Type, Action<Modifier>> _addMethods = new Dictionary<Type, Action<Modifier>>(); 
    
    // Dictionary to store the remove modifier methods
    private Dictionary<Type, Action<Modifier>> _removeMethods = new Dictionary<Type, Action<Modifier>>();


    // Register an add method for a specific Modifier type
    public void RegisterAddMethod<T>(Action<T> addMethod) where T : Modifier
    {
        Type type = typeof(T);
        
        /* In the add methods dictionary, we are adding the invoking of "addMethod" where modifier is the argument to pass to it,
        which is the modifier we want to add (ex. We are inserting AddDamageModifier(DamageModifier modifier) to the dictionary */
        _addMethods[type] = modifier => addMethod.Invoke((T)modifier);
    }

    public void AddModifier<T>(T modifierToAdd) where T : Modifier
    {
        Type type = typeof(T);
        
        // Check and see if an add method was registered with this manager, if not then return false
        if (_addMethods.TryGetValue(type, out Action<Modifier> removeMethod))
        {
            removeMethod.Invoke(modifierToAdd);
        }
        else
        {
            Debug.LogError($"{gameObject.name} does not have {type} modifier that they can add. Or they haven't registered the add method.");
        }
    }

    // Register a remove method for a specific Modifier type
    public void RegisterRemoveMethod<T>(Action<T> removeMethod) where T : Modifier
    {
        Type type = typeof(T);
        
        /* In the remove methods dictionary, we are adding the invoking of "removeMethod" where modifier is the argument to pass to it,
        which is the modifier we want to remove (ex. We are inserting RemoveDamageModifier(DamageModifier modifier) to the dictionary */
        _removeMethods[type] = modifier => removeMethod.Invoke((T)modifier);
    }
    
    public void RemoveModifier<T>(T modifierToRemove) where T : Modifier
    {
        // Find and invoke the remove method for the type T
        Type type = typeof(T);
        
        // Check and see if a remove method was registered with this manager, if not then return false
        if (_removeMethods.TryGetValue(type, out Action<Modifier> removeMethod))
        {
            removeMethod.Invoke(modifierToRemove);
        }
        else
        {
            Debug.LogError($"{gameObject.name} does not have {type} modifier that they can remove. Or they haven't registered the remove method.");
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Remove a modifier from a gameObject after "delay" amount of seconds.
    /// 
    public void RemoveModifierAfterDelay<T>(T modifierToRemove, float delay) where T : Modifier
    {
        StartCoroutine(RemoveDelayCoroutine(modifierToRemove, delay));
    }

    // Wait a few seconds before removing the modifier from the gameObject
    private IEnumerator RemoveDelayCoroutine<T>(T modifierToRemove, float delay) where T : Modifier
    {
        yield return new WaitForSeconds(delay);

        RemoveModifier(modifierToRemove);
    }
    
}
