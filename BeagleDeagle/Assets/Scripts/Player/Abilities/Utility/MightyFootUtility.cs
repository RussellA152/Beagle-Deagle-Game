using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MightyFootUtility : UtilityAbility<MightyFootUtilityData>
{
    private int PoolKey;
    
    protected override void Start()
    {
        base.Start();
        
        PoolKey = currentUtilityData.mightyFootPrefab.GetComponent<IPoolable>().PoolKey;

    }
    protected override void UtilityAction(GameObject player)
    {
        // Fetch a grenade from the object pool
        GameObject mightyFootGameObject = ObjectPooler.instance.GetPooledObject(PoolKey);
        
        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;

        MightyFootBullet bulletComponent = mightyFootGameObject.GetComponent<MightyFootBullet>();

        StatusEffect<StunData> stunComponent = mightyFootGameObject.GetComponent<StatusEffect<StunData>>();
        
        //StatusEffect<KnockBackData> knockBackComponent = mightyFootGameObject.GetComponent<StatusEffect<KnockBackData>>();

        stunComponent.UpdateScriptableObject(currentUtilityData.stunData);
        
        //knockBackComponent.UpdateScriptableObject(currentUtilityData.knockBackData);

        // Give MightyFoot the scriptable object it needs
        bulletComponent.UpdateScriptableObject(currentUtilityData.mightyFootData);

        bulletComponent.UpdateDamageAndPenetrationValues(currentUtilityData.abilityDamage, currentUtilityData.mightyFootData.numEnemiesCanHit);
        
        // Tell the bullet that the player is the transform that shot it
        bulletComponent.UpdateWhoShotThisBullet(transform);

        mightyFootGameObject.transform.position = (Vector2)player.transform.position + aimDirection; //+ new Vector2(offset.x, offset.y);

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        mightyFootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // Reenable the projectile
        mightyFootGameObject.SetActive(true);
    }

}
