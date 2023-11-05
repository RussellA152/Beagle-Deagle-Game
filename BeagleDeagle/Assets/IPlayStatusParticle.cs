using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayStatusParticle
{

    PoolableParticle ActiveParticle { get; }
    
    ///-///////////////////////////////////////////////////////////
    /// Play a particle effect on this gameObject (i.e. DOT effect, movement penalty or boost effect,).
    /// It can be overriden at the moment.
    public void PlayStatusParticleEffect(PoolableParticle particleEffect);

    ///-///////////////////////////////////////////////////////////
    /// Particle effects should play on top of the gameObject, which gives the impression that
    /// the gameObject is associated with the particle effect (ex. an enemy is bleeding)
    //public void StickToGameObject(PoolableParticle particleEffect);

}
