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
        explosivePrefab.transform.position = transform.position;
        
        explosivePrefab.SetActive(true);
        
        explosiveScript.Explode();
        
        Debug.Log("EXPLODE!");

        
        //GetComponent<IHealth>().ModifyHealth(-1000f);
    }
    
}
