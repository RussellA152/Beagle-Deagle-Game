using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifierWithParticle
{

    ///-///////////////////////////////////////////////////////////
    /// Play a particle effect on this gameObject (i.e. DOT effect, movement penalty or boost effect,).
    /// It can be overriden at the moment.
    public IEnumerator StartParticleEffect(PoolableParticle particleEffect, Modifier modifier);
    

}
