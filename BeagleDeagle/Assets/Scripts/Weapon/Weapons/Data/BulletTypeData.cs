using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BulletTypeData : ScriptableObject
{
    public abstract GameObject GetBulletPrefab();
    
    public abstract BulletData GetBulletData();
    

}
