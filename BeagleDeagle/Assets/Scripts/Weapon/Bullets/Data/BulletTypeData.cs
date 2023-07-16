using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewBulletType", menuName = "ScriptableObjects/BulletType")]
public class BulletTypeData : ScriptableObject
{
    [RestrictedPrefab(typeof(IBulletUpdatable))]
    public GameObject bulletPrefab;
    
    public BulletData bulletData;
    
    // TODO: Make this a list of StatusEffectUpdaters?
    public List<StatusEffectData> statusEffects = new List<StatusEffectData>();
    
    
    // TODO: Make MightyFoot use this script or deriving scripts, also create ExplosiveType and probably AOEType
    
    
    // Summary: What I am trying to do is have a bullet get updated with the bullet information, and also any other effects it may have (ex. fire, stun, etc.)
    
    
    ///-///////////////////////////////////////////////////////////
    /// Pass in a bullet, then give it the data it requires.
    /// Then, return it back to the gun.
    /// 
    public IBulletUpdatable UpdateBulletWithData(GameObject bullet, GameObject activator)
    {
        UpdateUniqueProperties(bullet);
        
        // if (statusEffects.Count > 0)
        // {
        //     foreach (StatusEffectData statusEffects in statusEffects)
        //     {
        //         statusEffects.UpdateStatusEffects(bullet, activator);
        //     }
        // }

        return bullet.GetComponent<IBulletUpdatable>();
    }

    private GameObject UpdateUniqueProperties(GameObject bullet)
    {
        IBulletUpdatable projectile = bullet.GetComponent<IBulletUpdatable>();
        
        projectile.UpdateScriptableObject(bulletData);

        return bullet;
    }

}
