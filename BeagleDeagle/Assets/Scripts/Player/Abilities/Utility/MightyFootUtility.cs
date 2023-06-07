using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMightyFoot", menuName = "ScriptableObjects/Ability/Utility/MightyFoot")]
public class MightyFootUtility : UtilityAbilityData
{
    [Header("Prefab to Spawn")]
    public GameObject prefab;

    [Header("Projectile Data")]
    public MightyFootBullet mightyFootData;

    [Header("Offset From Player Position")]
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

        // Spawn the mighty foot at the direction of the player
        mightyFootGameObject = SpawnAtPlayerDirection(mightyFootGameObject, player);

        // Reenable the projectile
        mightyFootGameObject.SetActive(true);
    }

    public override GameObject SpawnAtPlayerDirection(GameObject objectToSpawn, GameObject player)
    {
        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;

        Bullet bulletComponent = objectToSpawn.GetComponent<Bullet>();

        // Give MightyFoot the scriptable object it needs
        bulletComponent.UpdateProjectileData(mightyFootData);

        objectToSpawn.transform.position = (Vector2)player.transform.position + aimDirection; //+ new Vector2(offset.x, offset.y);

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        objectToSpawn.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        return objectToSpawn;
    }
}
