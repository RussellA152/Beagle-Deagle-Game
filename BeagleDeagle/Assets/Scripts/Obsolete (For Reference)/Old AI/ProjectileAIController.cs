using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAIController : AIController 
{
    private ProjectileEnemyData projectileScriptableObject;

    public override void UpdateScriptableObject(EnemyData scriptableObject)
    {
        projectileScriptableObject = scriptableObject as ProjectileEnemyData;

        if(projectileScriptableObject == null)
        {
            Debug.Log(gameObject.name + " failed to cast scriptableObject!" );
        }
        
    }


}
