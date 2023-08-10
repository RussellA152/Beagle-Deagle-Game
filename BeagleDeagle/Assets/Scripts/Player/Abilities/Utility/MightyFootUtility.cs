using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MightyFootUtility : UtilityAbility<MightyFootUtilityData>
{
    private int PoolKey;

    private TopDownMovement _playerMovementScript;
    
    protected override void Start()
    {
        base.Start();
        
        PoolKey = currentUtilityData.bulletType.bulletPrefab.GetComponent<IPoolable>().PoolKey;

        _playerMovementScript = gameObject.GetComponent<TopDownMovement>();

    }
    protected override void UtilityAction()
    {
        // Fetch a grenade from the object pool
        GameObject mightyFootGameObject = ObjectPooler.Instance.GetPooledObject(PoolKey);
        
        // Find direction that player is looking in
        Vector2 aimDirection = _playerMovementScript.ReturnPlayerDirection().normalized;

        MightyFootBullet bulletComponent = mightyFootGameObject.GetComponent<MightyFootBullet>();
        

        // Give MightyFoot the scriptable object it needs
        bulletComponent.UpdateScriptableObject(currentUtilityData.bulletType.bulletData);

        bulletComponent.GetComponent<IStatusEffect>().UpdateWeaponType(currentUtilityData.bulletType);
        
        bulletComponent.UpdateDamageAndPenetrationValues(currentUtilityData.abilityDamage, currentUtilityData.numberOfEnemiesCanHit);
        
        // Tell the bullet that the player is the transform that shot it
        bulletComponent.UpdateWhoShotThisBullet(transform);
        
        

        mightyFootGameObject.transform.position = (Vector2) (gameObject.transform.position) + aimDirection + new Vector2(currentUtilityData.offset.x, currentUtilityData.offset.y);

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        mightyFootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // Reenable the projectile
        mightyFootGameObject.SetActive(true);
    }

}
