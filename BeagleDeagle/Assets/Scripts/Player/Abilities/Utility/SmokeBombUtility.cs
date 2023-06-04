using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility/SmokeBomb")]
public class SmokeBombUtility : UtilityAbilityData
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private SmokeGrenade smokeGrenadeData;

    private int poolKey;

    private void OnEnable()
    {
        poolKey = prefab.GetComponent<IPoolable>().PoolKey;
    }

    public override void ActivateUtility(ObjectPooler objectPool, GameObject player)
    {
        Debug.Log("Throw smoke grenade!");

        // Fetch a grenade from the object pool
        GameObject grenade = objectPool.GetPooledObject(poolKey);

        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;

        Grenade grenadeComponent = grenade.GetComponent<Grenade>();

        // Make grenade spawn at player's position
        grenade.transform.position = player.transform.position;

        grenade.SetActive(true);

        grenadeComponent.UpdateThrowableData(smokeGrenadeData);

        // Throw grenade in the direction player is facing
        grenadeComponent.ActivateGrenade(aimDirection);
    }
}
