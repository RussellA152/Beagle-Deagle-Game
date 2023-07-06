using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletType", menuName = "ScriptableObjects/BulletType/RegularBullet")]
public class RegularBulletTypeData : BulletTypeData
{
    [Header("Bullet To Shoot")]
    [RestrictedPrefab(typeof(RegularBullet))]
    // What bullet is spawned when shooting?
    public GameObject bulletPrefab; 
    
    // What data will this bullet use?
    public RegularBulletData bulletData;


    public override GameObject GetBulletPrefab()
    {
        return bulletPrefab;
    }

    public override BulletData GetBulletData()
    {
        return bulletData;
    }
}
