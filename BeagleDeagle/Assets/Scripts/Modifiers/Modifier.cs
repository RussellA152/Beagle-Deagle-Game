using System.Collections.Generic;
using UnityEngine;
public class Modifier
{
    // The name of the object or item that is applying this modifier
    public string modifierName;

    [Header("Particle Effect")]
    [RestrictedPrefab(typeof(PoolableParticle))]
    public GameObject particleEffect;

    public bool StickToGameObject;
    
    /* Should modifier play immediately when applied to target? If not, then ModifierManager will not play it automatically
     Another script will need to play it. */
    
    public bool PlayOnModifierApplied;
    
    ///-///////////////////////////////////////////////////////////
    /// A modifier is considered "null" if it at least has a name. Don't add it to lists if 
    /// if its null
    /// 
    public bool IsModifierNameValid()
    {
        return !string.IsNullOrEmpty(modifierName);
    }

}

