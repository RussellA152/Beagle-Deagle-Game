using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnBuffBar : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    private ModifierManager _modifierManager;
    private Modifier _buffModifier;
    private Sprite _buffIcon;

    public void SetBuffIcon(Sprite buffIcon)
    {
        _buffIcon = buffIcon;
    }

    public void SetBuffModifier(Modifier buffModifier)
    {
        _buffModifier = buffModifier;
    }

    public void SetModifierManager(ModifierManager modifierManager)
    {
        _modifierManager = modifierManager;
    }

    public void ShowBuffPermanently()
    {
        playerEvents.InvokePassiveActivated(_buffIcon);
    }
    public void ShowBuffIconWithoutDuration()
    {
        playerEvents.InvokePassiveActivated(_buffIcon);

        _modifierManager.onModifierWasRemoved += RemoveIconFromBuffBar;
    }

    public void ShowBuffIconWithDuration(float displayDuration)
    {
        playerEvents.InvokePassiveWithDurationActivated(_buffIcon, displayDuration);

        _modifierManager.onModifierWasRemoved += RemoveIconFromBuffBar;
    }

    public void RemoveIconFromBuffBar()
    {
        playerEvents.InvokePassiveDeactivated(_buffIcon);
    }
    
    private void RemoveIconFromBuffBar(Modifier modifierRemoved)
    {
        if (modifierRemoved != _buffModifier) return;
        
        Debug.Log("Remove my icon! From: " + gameObject.name);
        
        playerEvents.InvokePassiveDeactivated(_buffIcon);
        _modifierManager.onModifierWasRemoved -= RemoveIconFromBuffBar;
        
    }
}
