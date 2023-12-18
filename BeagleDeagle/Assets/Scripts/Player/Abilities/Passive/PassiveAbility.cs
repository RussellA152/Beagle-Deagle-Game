using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility: MonoBehaviour
{
    protected GameObject Player;

    protected virtual void OnEnable()
    {
        // Passive ability scripts have another parent, so the parent.parent would be the Player gameObject with all the components attached
        Player = transform.parent.parent.gameObject;
        
        ActivatePassive();
    }

    protected virtual void OnDisable()
    {
        RemovePassive();
    }

    protected abstract void ActivatePassive();

    protected abstract void RemovePassive();
    
}
