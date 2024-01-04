using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAttack : AIAttack<BomberEnemyData>
{
    public override void InitiateAttack()
    {
        // Fetch an explosive GameObject from the object pooler   
        GameObject explosivePrefab = Instantiate(enemyScriptableObject.explosivePrefab);
        
        explosivePrefab.SetActive(false);

        // Tell explosive type to give the explosiveData scriptableObject to this prefab
        IExplosiveUpdatable explosiveScript = explosivePrefab.GetComponent<IExplosiveUpdatable>();
        
        // Set the damage and duration of the explosive
        explosiveScript.SetDamage(enemyScriptableObject.attackDamage);
        explosiveScript.SetDuration(enemyScriptableObject.explosiveDuration);
        
        //explosivePrefab.transform.position = transform.position;
        
        explosivePrefab.SetActive(true);
        
        // Give bomber's explosive the data it needs
        explosiveScript.UpdateScriptableObject(enemyScriptableObject.explosiveData);
        
        // Bomber enemies do not use detonation time
        explosiveScript.Activate(transform.position);
        
    }
    
}
