using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility<T> : MonoBehaviour where T: ScriptableObject
{
    protected GameObject player;
    
    [SerializeField] 
    protected T passiveData;

    private void Start()
    {
        ActivatePassive();
    }

    protected virtual void OnEnable()
    {
        player = transform.parent.gameObject;
    }

    protected virtual void OnDisable()
    {
        RemovePassive();
    }

    protected abstract void ActivatePassive();

    protected abstract void RemovePassive();

    public void UpdateScriptableObject(T scriptableObject)
    {
        passiveData = scriptableObject;
    }
}
