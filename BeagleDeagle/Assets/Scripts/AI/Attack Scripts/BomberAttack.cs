using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAttack : AIAttack<BomberEnemyData>
{
    private IHealth _healthScript;
    private void Start()
    {
        _healthScript = GetComponent<IHealth>();
    }

    public override void InitiateAttack()
    {
        // Fetch an explosive GameObject from the object pooler   
        GameObject explosivePrefab = ObjectPooler.instance.GetPooledObject(enemyScriptableObject.explosiveType.explosivePrefab.GetComponent<IPoolable>().PoolKey);

        // Tell explosive type to give the explosiveData scriptableObject to this prefab
        IExplosiveUpdatable explosiveScript = enemyScriptableObject.explosiveType.UpdateExplosiveWithData(explosivePrefab);
        
        // Set the damage and duration of the explosive
        explosiveScript.SetDamage(enemyScriptableObject.attackDamage);
        explosiveScript.SetDuration(enemyScriptableObject.explosiveDuration);
        
        explosivePrefab.transform.position = transform.position;
        explosivePrefab.SetActive(true);
        
        // Bomber enemies do not use detonation time
        explosiveScript.Activate(transform.position);

        // Bomber enemy will kill themselves on their attack
        _healthScript.ModifyHealth(-1f * enemyScriptableObject.maxHealth);
    }
    
}
