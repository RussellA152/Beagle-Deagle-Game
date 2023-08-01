using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeUtility : UtilityAbility<SmokeGrenadeUtilityData>
{
    private int PoolKey;
    
    //[SerializeField] 
    //private SmokeGrenadeUtilityData utilityData;
    
    
    protected override void Start()
    {
        base.Start();
        
        PoolKey = currentUtilityData.smokeGrenadePrefab.GetComponent<IPoolable>().PoolKey;

    }
    
    protected override void UtilityAction(GameObject player)
    {
        // Fetch a grenade from the object pool
        GameObject grenade = ObjectPooler.instance.GetPooledObject(PoolKey);
        
        
        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;
        
        Debug.Log(aimDirection);

        IExplosiveUpdatable areaGrenadeComponent = grenade.GetComponent<IExplosiveUpdatable>();
        
        areaGrenadeComponent.SetDamage(currentUtilityData.abilityDamage);
        areaGrenadeComponent.SetDuration(currentUtilityData.duration);
        
        Debug.Log(areaGrenadeComponent);

        StatusEffect<SlowData> slowComponent = grenade.GetComponentInChildren<StatusEffect<SlowData>>();

        Debug.Log(slowComponent);
        
        slowComponent.UpdateScriptableObject(currentUtilityData.slowData);

        // Make grenade spawn at player's position
        grenade.transform.position = player.transform.position;

        grenade.SetActive(true);

        areaGrenadeComponent.UpdateScriptableObject(currentUtilityData.utilityExplosiveData);

        // Throw grenade in the direction player is facing
        areaGrenadeComponent.Activate(aimDirection);
        
    }
    
}
