using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StunModifier : Modifier
{
    [Range(0.1f, 30f)]
    public float stunDuration;
    
    [RestrictedPrefab(typeof(PoolableParticle))]
    public GameObject particleEffect;
    
    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to a entity's movement speed.
    /// 
    public StunModifier(string name, float duration)
    {
        modifierName = name;
        stunDuration = duration;
    }
}
