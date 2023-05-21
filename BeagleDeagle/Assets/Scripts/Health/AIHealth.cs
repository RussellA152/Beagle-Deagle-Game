using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health, IEnemyDataUpdatable
{
    public void UpdateScriptableObject(EnemyData scriptableObject)
    {
        characterData = scriptableObject;
    }

}
