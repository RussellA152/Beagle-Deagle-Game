using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeUtilityAbility : UtilityAbility<SmokeGrenadeUtilityData>
{
    protected override void UtilityAction(ObjectPooler objectPool, GameObject player)
    {
        Debug.Log("Throw smoke grenade!");

        // Fetch a grenade from the object pool
        GameObject grenade = objectPool.GetPooledObject(PoolKey);
        
        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;

        Grenade grenadeComponent = grenade.GetComponent<Grenade>();

        // Make grenade spawn at player's position
        grenade.transform.position = player.transform.position;

        grenade.SetActive(true);

        grenadeComponent.UpdateExplosiveData(utilityData.smokeGrenadeData);

        // Throw grenade in the direction player is facing
        grenadeComponent.ActivateGrenade(aimDirection);
        
    }
    
}
