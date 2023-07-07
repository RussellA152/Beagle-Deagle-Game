using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MightyFootUtility : MonoBehaviour
{
    private int PoolKey;
    
    [SerializeField] 
    private MightyFootUtilityData utilityData;

    [SerializeField] 
    private StunData stunData;

    [SerializeField] private KnockBackData knockBackData;
    private void Start()
    {
        PoolKey = utilityData.mightyFootPrefab.GetComponent<IPoolable>().PoolKey;
        
        //base.Start();
    }
    public void UtilityAction(GameObject player)
    {
        // Fetch a grenade from the object pool
        GameObject mightyFootGameObject = ObjectPooler.instance.GetPooledObject(PoolKey);
        
        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;

        MightyFootBullet bulletComponent = mightyFootGameObject.GetComponent<MightyFootBullet>();

        StatusEffect<StunData> stunComponent = mightyFootGameObject.GetComponent<StatusEffect<StunData>>();
        
        StatusEffect<KnockBackData> knockBackComponent = mightyFootGameObject.GetComponent<StatusEffect<KnockBackData>>();

        stunComponent.UpdateScriptableObject(stunData);
        knockBackComponent.UpdateScriptableObject(knockBackData);

        // Give MightyFoot the scriptable object it needs
        bulletComponent.UpdateScriptableObject(utilityData.mightyFootData);

        bulletComponent.UpdateWeaponValues(utilityData.abilityDamage, utilityData.mightyFootData.numEnemiesCanHit);
        
        // Tell the bullet that the player is the transform that shot it
        bulletComponent.UpdateWhoShotThisBullet(transform);

        mightyFootGameObject.transform.position = (Vector2)player.transform.position + aimDirection; //+ new Vector2(offset.x, offset.y);

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        mightyFootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // Reenable the projectile
        mightyFootGameObject.SetActive(true);
    }

}
