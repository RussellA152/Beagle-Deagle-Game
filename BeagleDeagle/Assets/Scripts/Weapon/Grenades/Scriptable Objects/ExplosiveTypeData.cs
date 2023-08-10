using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosiveType", menuName = "ScriptableObjects/ExplosiveType")]
public class ExplosiveTypeData : WeaponType
{
    [RestrictedPrefab(typeof(IExplosiveUpdatable))]
    public GameObject explosivePrefab;

    public ExplosiveData explosiveData;
    
    //public List<StatusEffectData> statusEffects = new List<StatusEffectData>();
    
    ///-///////////////////////////////////////////////////////////
    /// Pass in a explosive, then give it the data it requires.
    /// Then, return it back to the activator.
    /// 
    public IExplosiveUpdatable UpdateExplosiveWithData(GameObject explosive)
    {
        IExplosiveUpdatable explosiveScript = explosive.GetComponent<IExplosiveUpdatable>();
        explosiveScript.UpdateScriptableObject(explosiveData);
        

        return explosiveScript;
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Return the first instance of a specific status effect type, inside of the explosive type's list
    /// 
    // public T GetExplosiveTypeStatusEffect<T>()
    // {
    //     return statusEffects.OfType<T>().FirstOrDefault();
    // }
    
}
