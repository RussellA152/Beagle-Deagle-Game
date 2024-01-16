using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownPowerUp : PowerUp
{
    private ShowOnBuffBar _showOnBuffBar;
    private ModifierManager _modifierManager;

    [SerializeField] private AbilityCooldownModifierData abilityCooldownModifierData;

    [SerializeField, Range(0.1f, 60f)] 
    private float cooldownBuffDuration;

    [SerializeField] private Sprite icon;

    protected override void Awake()
    {
        base.Awake();
        
        _showOnBuffBar = GetComponent<ShowOnBuffBar>();
        
        _showOnBuffBar.SetBuffIcon(icon);
        _showOnBuffBar.SetBuffModifier(abilityCooldownModifierData.utilityCooldownModifier);
    }

    protected override void OnPickUp(GameObject receiverGameObject)
    {
        _modifierManager = receiverGameObject.GetComponent<ModifierManager>();
        
        _showOnBuffBar.SetModifierManager(_modifierManager);

        if (_modifierManager.DoesEntityContainModifier(abilityCooldownModifierData.utilityCooldownModifier) ||
            _modifierManager.DoesEntityContainModifier(abilityCooldownModifierData.ultimateCooldownModifier))
        {
            _modifierManager.RefreshRemoveModifierTimer(abilityCooldownModifierData.utilityCooldownModifier, cooldownBuffDuration);
            _modifierManager.RefreshRemoveModifierTimer(abilityCooldownModifierData.ultimateCooldownModifier, cooldownBuffDuration);
        }
        else
        {
            _modifierManager.AddModifierOnlyForDuration(abilityCooldownModifierData.utilityCooldownModifier, cooldownBuffDuration);
            _modifierManager.AddModifierOnlyForDuration(abilityCooldownModifierData.ultimateCooldownModifier, cooldownBuffDuration);
        }
        
        _showOnBuffBar.ShowBuffIconWithDuration(cooldownBuffDuration);
        
    }
    
    
}
