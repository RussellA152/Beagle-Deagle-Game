using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MightyFootUtility : UtilityAbility<MightyFootUtilityData>
{
    //private int PoolKey;

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
        
        bulletComponent.UpdateDamageAndPenetrationValues(UtilityData.abilityDamage, UtilityData.mightyFootData.numberOfEnemiesCanHit);
        
        // Tell the bullet that the player is the transform that shot it
        bulletComponent.UpdateWhoShotThisBullet(transform);
        

        mightyFootGameObject.transform.position = (Vector2) (gameObject.transform.position) + aimDirection + new Vector2(UtilityData.offset.x, UtilityData.offset.y);

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        mightyFootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // Reenable the projectile
        mightyFootGameObject.SetActive(true);
        
        bulletComponent.ActivateBullet();
    }
    
}
