using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAttack : AIAttack<BomberEnemyData>
{

    public override void InitiateAttack()
    {
        //ExplosiveData explosivePrefab = enemyScriptableObject.explosiveType;

        GameObject explosivePrefab = ObjectPooler.instance.GetPooledObject(enemyScriptableObject.explosiveType.explosivePrefab.GetComponent<IPoolable>().PoolKey);

        IExplosiveUpdatable explosiveScript = enemyScriptableObject.explosiveType.UpdateExplosiveWithData(explosivePrefab);
        
        explosiveScript.SetDamage(enemyScriptableObject.attackDamage);
        explosiveScript.SetDuration(enemyScriptableObject.explosiveDuration);
        
        explosivePrefab.transform.position = transform.position;
        explosivePrefab.SetActive(true);
        
        // Bomber enemies do not use detonation time
        explosiveScript.Activate(transform.position);
        
        Debug.Log("EXPLODE!");

        // Bomber enemy will kill themselves on their attack
        GetComponent<IHealth>().ModifyHealth(-1f * enemyScriptableObject.maxHealth);
    }
    
}
