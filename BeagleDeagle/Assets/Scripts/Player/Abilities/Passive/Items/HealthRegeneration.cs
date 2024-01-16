using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration : PassiveAbility
{
    [SerializeField] private HealthRegenData healthRegenData;
    
    private IHealth _healthScript;
    
    private bool _allowRegeneration;
    
    protected override void Awake()
    {
        base.Awake();

        _healthScript = Player.GetComponent<IHealth>();
    }
    
    private void Update()
    {
        // Constantly heal the gameObject (this doesn't have a health threshold)
        if(_allowRegeneration && _healthScript.IsHealthBelowPercentage(healthRegenData.regenThreshold)  && !_healthScript.IsDead())
            _healthScript.ModifyHealth(healthRegenData.regenRate * Time.deltaTime);
    }
    
    protected override void ActivatePassive()
    {
        _allowRegeneration = true;
        ShowOnBuffBar.ShowBuffPermanently();
    }
    
    protected override void RemovePassive()
    {
        _allowRegeneration = false;
        
        ShowOnBuffBar.RemoveIconFromBuffBar();
    }

}
