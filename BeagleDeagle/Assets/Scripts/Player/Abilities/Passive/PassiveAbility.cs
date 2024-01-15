using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility: MonoBehaviour
{
    [SerializeField] protected PlayerEvents playerEvents;
    
    protected GameObject Player;
    
    [SerializeField] protected PassiveAbilityData passiveAbilityData;

    protected ShowOnBuffBar ShowOnBuffBar;

    protected virtual void Awake()
    {
        // Passive ability scripts have another parent, so the parent.parent would be the Player gameObject with all the components attached
        Player = transform.parent.parent.gameObject;
        
        ShowOnBuffBar = GetComponent<ShowOnBuffBar>();
        
        ShowOnBuffBar.SetBuffIcon(passiveAbilityData.abilityIcon);
    }

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void Start()
    {
        ActivatePassive();
    }

    protected virtual void OnDisable()
    {
        RemovePassive();
    }

    protected abstract void ActivatePassive();

    protected abstract void RemovePassive();
    
}
