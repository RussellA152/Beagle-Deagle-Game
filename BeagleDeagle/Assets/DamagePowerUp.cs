using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerUp : PowerUp
{
    private ShowOnBuffBar _showOnBuffBar;
    private ModifierManager _modifierManager;
    
    [SerializeField] private DamageModifier damageModifier;

    [Range(0.1f, 60f)]
    [SerializeField] private float damageBuffDuration;
    
    
    [SerializeField] private Sprite icon;
    
    protected override void Awake()
    {
        base.Awake();

        _showOnBuffBar = GetComponent<ShowOnBuffBar>();
        
        _showOnBuffBar.SetBuffIcon(icon);
        _showOnBuffBar.SetBuffModifier(damageModifier);
        
    }

    protected override void OnPickUp(GameObject receiverGameObject)
    {
        _modifierManager = receiverGameObject.GetComponent<ModifierManager>();
        
        _showOnBuffBar.SetModifierManager(_modifierManager);
        
        _modifierManager.AddModifierOnlyForDuration(damageModifier, damageBuffDuration);
        
        _showOnBuffBar.ShowBuffIconWithDuration(damageBuffDuration);
        
    }
}
