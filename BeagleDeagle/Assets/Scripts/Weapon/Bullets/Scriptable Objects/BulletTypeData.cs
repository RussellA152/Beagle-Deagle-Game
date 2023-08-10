using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewBulletType", menuName = "ScriptableObjects/BulletType")]
public class BulletTypeData : WeaponType
{
    [RestrictedPrefab(typeof(IBulletUpdatable))]
    public GameObject bulletPrefab;
    
    public BulletData bulletData;
    
    //public List<StatusEffectData> statusEffects = new List<StatusEffectData>();
    
    
    ///-///////////////////////////////////////////////////////////
    /// Pass in a bullet, then give it the data it requires.
    /// Then, return it back to the activator.
    /// 
    public IBulletUpdatable UpdateBulletWithData(GameObject bullet, GameObject activator)
    {
        IBulletUpdatable projectile = bullet.GetComponent<IBulletUpdatable>();
        
        projectile.UpdateScriptableObject(bulletData);
        

        return projectile;
    }

    ///-///////////////////////////////////////////////////////////
    /// Return the first instance of a specific status effect type, inside of the bullet type's list
    /// 
    // public T GetBulletTypeStatusEffect<T>()
    // {
    //     return statusEffects.OfType<T>().FirstOrDefault();
    // }
}
