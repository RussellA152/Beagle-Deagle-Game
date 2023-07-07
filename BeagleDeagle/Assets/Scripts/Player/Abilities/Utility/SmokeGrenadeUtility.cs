using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeUtility : MonoBehaviour
{
    private int PoolKey;
    
    [SerializeField] 
    private SmokeGrenadeUtilityData utilityData;
    
    
    private void Start()
    {
        PoolKey = utilityData.smokeGrenadePrefab.GetComponent<IPoolable>().PoolKey;
        
        //base.Start();
    }
    
    public void UtilityAction(GameObject player)
    {
        // Fetch a grenade from the object pool
        GameObject grenade = ObjectPooler.instance.GetPooledObject(PoolKey);
        
        
        // Find direction that player is looking in
        Vector2 aimDirection = player.GetComponent<TopDownMovement>().ReturnPlayerDirection().normalized;
        
        Debug.Log(aimDirection);

        AreaGrenade areaGrenadeComponent = grenade.GetComponent<AreaGrenade>();
        
        Debug.Log(areaGrenadeComponent);

        StatusEffect<SlowData> slowComponent = grenade.GetComponentInChildren<StatusEffect<SlowData>>();

        Debug.Log(slowComponent);
        
        slowComponent.UpdateScriptableObject(utilityData.slowData);

        // Make grenade spawn at player's position
        grenade.transform.position = player.transform.position;

        grenade.SetActive(true);

        areaGrenadeComponent.UpdateScriptableObject(utilityData.utilityGrenadeData);

        // Throw grenade in the direction player is facing
        areaGrenadeComponent.ActivateGrenade(aimDirection);
        
    }
    
}
