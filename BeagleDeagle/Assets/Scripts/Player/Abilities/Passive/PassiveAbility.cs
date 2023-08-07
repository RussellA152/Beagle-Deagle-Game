using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility<T> : MonoBehaviour where T: ScriptableObject
{
    protected GameObject Player;
    
    [SerializeField]  protected T passiveData;

    private void Start()
    {
        ActivatePassive();
    }

    protected virtual void OnEnable()
    {
        // Passive ability scripts have another parent, so the parent.parent would be the Player gameObject with all the components attached
        Player = transform.parent.parent.gameObject;
    }

    protected virtual void OnDisable()
    {
        RemovePassive();
    }

    protected abstract void ActivatePassive();

    protected abstract void RemovePassive();

    public void UpdateScriptableObject(T scriptableObject)
    {
        if (scriptableObject is T)
        {
            passiveData = scriptableObject as T;
        }
        else
        {
            Debug.LogError("ERROR WHEN UPDATING SCRIPTABLE OBJECT! " + scriptableObject + " IS NOT A " + typeof(T));
        }
    }
}
