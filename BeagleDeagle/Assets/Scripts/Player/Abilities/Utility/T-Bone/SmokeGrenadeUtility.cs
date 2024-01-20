using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeUtility : UtilityAbility<SmokeGrenadeUtilityData>
{
    private TopDownMovement _playerMovementScript;

    protected override void Start()
    {
        base.Start();

        _playerMovementScript = gameObject.GetComponent<TopDownMovement>();
        
    }
    
    protected override void UtilityAction()
    {

        GameObject grenade = Instantiate(UtilityData.smokeGrenadePrefab);
        
        // Find direction that player is looking in
        Vector2 aimDirection = _playerMovementScript.ReturnPlayerDirection().normalized;
        

        IExplosiveUpdatable areaGrenadeComponent = grenade.GetComponent<IExplosiveUpdatable>();
        
        // Give smoke grenade any extra modifiers (ex. giving grenade "bonusExplosiveRadius")
        foreach (IHasMiscellaneousModifier hasMiscellaneousModifier in grenade.GetComponents<IHasMiscellaneousModifier>())
        {
            hasMiscellaneousModifier.GiveMiscellaneousModifierList(MiscellaneousModifierList);
        }
        
        areaGrenadeComponent.SetDamage(UtilityData.abilityDamage * BonusUtilityDamage);
        areaGrenadeComponent.SetDuration(UtilityData.duration);

        // Make grenade spawn at player's position
        grenade.transform.position = gameObject.transform.position;

        grenade.SetActive(true);

        foreach (IStatusEffect statusEffect in grenade.GetComponents<IStatusEffect>())
        {
            statusEffect.UpdateWeaponType(UtilityData.statusEffects);
        }
        
        areaGrenadeComponent.UpdateScriptableObject(UtilityData.smokeGrenadeData);
        

        // Throw grenade in the direction player is facing
        areaGrenadeComponent.Activate(aimDirection);
        
    }

}
