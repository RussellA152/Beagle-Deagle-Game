using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeUtility : UtilityAbility<SmokeGrenadeUtilityData>
{
    private int _poolKey;

    private TopDownMovement _playerMovementScript;

    protected override void Start()
    {
        base.Start();
        
        _poolKey = utilityData.smokeGrenadePrefab.GetComponent<IPoolable>().PoolKey;

        _playerMovementScript = gameObject.GetComponent<TopDownMovement>();
        
    }
    
    protected override void UtilityAction()
    {
        // Fetch a grenade from the object pool
        GameObject grenade = ObjectPooler.Instance.GetPooledObject(_poolKey);
        
        
        // Find direction that player is looking in
        Vector2 aimDirection = _playerMovementScript.ReturnPlayerDirection().normalized;
        
        Debug.Log(aimDirection);

        IExplosiveUpdatable areaGrenadeComponent = grenade.GetComponent<IExplosiveUpdatable>();
        
        areaGrenadeComponent.SetDamage(utilityData.abilityDamage);
        areaGrenadeComponent.SetDuration(utilityData.duration); ;

        // Make grenade spawn at player's position
        grenade.transform.position = gameObject.transform.position;

        grenade.SetActive(true);

        foreach (IStatusEffect statusEffect in grenade.GetComponents<IStatusEffect>())
        {
            statusEffect.UpdateWeaponType(utilityData.statusEffects);
        }
        
        areaGrenadeComponent.UpdateScriptableObject(utilityData.smokeGrenadeData);
        

        // Throw grenade in the direction player is facing
        areaGrenadeComponent.Activate(aimDirection);
        
    }
    
}
