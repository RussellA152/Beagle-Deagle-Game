using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeUtility : MonoBehaviour
{
    private int PoolKey;
    
    [SerializeField] 
    private SmokeGrenadeUtilityData utilityData;

    [SerializeField] 
    private SlowData slowData;
    
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

        SmokeGrenade smokeGrenadeComponent = grenade.GetComponent<SmokeGrenade>();
        
        Debug.Log(smokeGrenadeComponent);

        StatusEffect<SlowData> slowComponent = grenade.GetComponentInChildren<StatusEffect<SlowData>>();

        Debug.Log(slowComponent);
        
        slowComponent.UpdateScriptableObject(slowData);

        // Make grenade spawn at player's position
        grenade.transform.position = player.transform.position;

        grenade.SetActive(true);

        smokeGrenadeComponent.UpdateScriptableObject(utilityData.smokeGrenadeData);

        // Throw grenade in the direction player is facing
        smokeGrenadeComponent.ActivateGrenade(aimDirection);
        
    }
    
}
