using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightyFootUtilityAbility : UtilityAbility<MightyFootUtilityData>
{
    protected override void UtilityAction(ObjectPooler objectPool, GameObject player)
    {
        // Fetch a grenade from the object pool
        GameObject mightyFootGameObject = objectPool.GetPooledObject(PoolKey);
        
        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;

        Bullet bulletComponent = mightyFootGameObject.GetComponent<Bullet>();

        // Give MightyFoot the scriptable object it needs
        bulletComponent.UpdateProjectileData(utilityData.mightyFootData);

        bulletComponent.UpdateWeaponValues(utilityData.abilityDamage, utilityData.mightyFootData.numEnemiesCanHit);

        mightyFootGameObject.transform.position = (Vector2)player.transform.position + aimDirection; //+ new Vector2(offset.x, offset.y);

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        mightyFootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // Reenable the projectile
        mightyFootGameObject.SetActive(true);

    }

}
