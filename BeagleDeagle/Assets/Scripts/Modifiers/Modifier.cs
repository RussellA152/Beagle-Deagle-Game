using System.Collections.Generic;
using UnityEngine;
public class Modifier
{
    // The name of the object or item that is applying this modifier
    public string modifierName;

    [RestrictedPrefab(typeof(PoolableParticle))]
    public GameObject particleEffect;

    ///-///////////////////////////////////////////////////////////
    /// A modifier is considered "null" if it at least has a name. Don't add it to lists if 
    /// if its null
    /// 
    public bool IsModifierNameValid()
    {
        return !string.IsNullOrEmpty(modifierName);
    }


}

