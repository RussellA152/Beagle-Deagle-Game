using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosiveType", menuName = "ScriptableObjects/ExplosiveType")]
public class ExplosiveTypeData : ScriptableObject
{
    public GameObject explosivePrefab;

    public ExplosiveData explosiveData;
    
    public List<StatusEffectData> statusEffects = new List<StatusEffectData>();
    
    ///-///////////////////////////////////////////////////////////
    /// Pass in a explosive, then give it the data it requires.
    /// Then, return it back to the activator.
    /// 
    public IExplosiveUpdatable UpdateExplosiveWithData(GameObject explosive)
    {
        IExplosiveUpdatable explosiveScript = explosive.GetComponent<IExplosiveUpdatable>();
        explosiveScript.UpdateScriptableObject(explosiveData);
        
        // if (statusEffects.Count > 0)
        // {
        //     foreach (StatusEffectData statusEffects in statusEffects)
        //     {
        //         statusEffects.UpdateStatusEffects(bullet, activator);
        //     }
        // }

        return explosiveScript;
    }
    
}
