using System.Collections.Generic;
using UnityEngine;
public class Modifier
{
    // The name of the object or item that is applying this modifier
    public string modifierName;

    [RestrictedPrefab(typeof(PoolableParticle))]
    public GameObject particleEffect;

}

