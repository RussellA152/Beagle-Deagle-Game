using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MightyFootUtility : UtilityAbility<MightyFootUtilityData>
{
    private TopDownMovement _playerMovementScript;
    
    protected override void Start()
    {
        base.Start();

        _playerMovementScript = gameObject.GetComponent<TopDownMovement>();

    }
    protected override void UtilityAction()
    {
        // Spawn a new MightyFoot gameObject
        GameObject mightyFootGameObject = Instantiate(UtilityData.mightyFootPrefab);
        
        // Find direction that player is looking in
        Vector2 aimDirection = _playerMovementScript.ReturnPlayerDirection().normalized;

        MightyFootBullet bulletComponent = mightyFootGameObject.GetComponent<MightyFootBullet>();
        

        // Give MightyFoot the scriptable object it needs
        bulletComponent.UpdateScriptableObject(UtilityData.mightyFootData);

        // Add stats to any status effects that the MightyFoot needs
        foreach (IStatusEffect statusEffect in bulletComponent.GetComponents<IStatusEffect>())
        {
            statusEffect.UpdateWeaponType(UtilityData.statusEffects);
        }
        
        // Give mighty foot any extra modifiers (ex. giving mighty foot "bonusStunDuration")
        foreach (IHasMiscellaneousModifier hasMiscellaneousModifier in bulletComponent.GetComponents<IHasMiscellaneousModifier>())
        {
            hasMiscellaneousModifier.GiveMiscellaneousModifierList(MiscellaneousModifierList);
        }
        
        bulletComponent.UpdateDamageAndPenetrationValues(UtilityData.abilityDamage, UtilityData.mightyFootData.numberOfEnemiesCanHit);
        
        // Tell the bullet that the player is the transform that shot it
        bulletComponent.UpdateWhoShotThisBullet(transform);

        Vector2 offsetAppliedToBullet = aimDirection;
        
        if (aimDirection.x >= 0)
        {
            offsetAppliedToBullet += new Vector2(UtilityData.offset.x, UtilityData.offset.y);
        }
        else
        {
            offsetAppliedToBullet += new Vector2(-UtilityData.offset.x, UtilityData.offset.y);
        }

        mightyFootGameObject.transform.position = (Vector2)gameObject.transform.position + offsetAppliedToBullet;

        // Adjust rotation of the mighty foot if true (Mighty Foot roundhouse doesn't use this)
        if (UtilityData.usePlayerAimAngleForRotation)
        {
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
            mightyFootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);
        }
        

        // Reenable the projectile
        mightyFootGameObject.SetActive(true);
        
        bulletComponent.ActivateBullet();
    }
    
}
