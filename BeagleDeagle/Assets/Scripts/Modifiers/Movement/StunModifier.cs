using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StunModifier : Modifier
{
    [Range(0.1f, 30f)]
    public float stunDuration;

    ///-///////////////////////////////////////////////////////////
    /// Stop an entity's movement for some time.
    /// 
    public StunModifier(string name, float duration)
    {
        modifierName = name;
        stunDuration = duration;
    }
}

[System.Serializable]
public class StunDurationModifier : Modifier
{
    [Range(0.1f, 30f)]
    public float bonusDuration;

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to the timer of a stun.
    /// 
    public StunDurationModifier(string name, float duration)
    {
        modifierName = name;
        bonusDuration = duration;
    }
}
