using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMightyFoot", menuName = "ScriptableObjects/Ability/Utility/MightyFoot")]
public class MightyFootUtility : UtilityAbilityData
{
    public GameObject prefab;

    public MightyFootBullet mightyFootData;

    public Vector2 offset; // Offset applied to Mighty Foot projectile when this ability is activated

    private int poolKey;

    private void OnEnable()
    {
        poolKey = prefab.GetComponent<IPoolable>().PoolKey;
    }

    public override void ActivateUtility(ObjectPooler objectPool, GameObject player)
    {
        // Fetch a grenade from the object pool
        GameObject mightyFootGameObject = objectPool.GetPooledObject(poolKey);

        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;

        //mightyFootGameObject.transform.position = aimDirection;

        Bullet bulletComponent = mightyFootGameObject.GetComponent<Bullet>();
        bulletComponent.UpdateProjectileData(mightyFootData);
        bulletComponent.UpdateWeaponValues(mightyFootData.damagePerHit, mightyFootData.numEnemiesCanHit);

        mightyFootGameObject.transform.position = (Vector2)player.transform.position + aimDirection; //+ new Vector2(offset.x, offset.y);

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        mightyFootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // Reenable the projectile
        mightyFootGameObject.SetActive(true);

    }
}
