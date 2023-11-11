using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectHandler : MonoBehaviour
{
    // All modifiers currently playing on this object
    private Dictionary<Modifier, PoolableParticle> _activeModifierParticles = new Dictionary<Modifier, PoolableParticle>();

    ///-///////////////////////////////////////////////////////////
    /// Play a particle effect associated with a modifier, if it has one.
    /// This is how to play particle effects like fire on an enemy.
    public void StartPlayingParticle(Modifier modifier, bool stickToObject)
    {
        if (modifier.particleEffect != null)
        {
            // Find the particle effect in object pool
            PoolableParticle particleToPlay = ObjectPooler.Instance.GetPooledObject(modifier.particleEffect.GetComponent<IPoolable>().PoolKey).GetComponent<PoolableParticle>();

            if (particleToPlay != null)
            {
                _activeModifierParticles.Add(modifier, particleToPlay);
        
                if(stickToObject)
                    particleToPlay.StickParticleToTransform(transform);
                else
                    particleToPlay.PlaceParticleOnTransform(transform);
            
                particleToPlay.PlayAllParticles(1f);
            }
        }
        
        
    }

    ///-///////////////////////////////////////////////////////////
    /// Stop playing a particle effect associated with a modifier. For instance, if
    /// a slow modifier expires on an enemy, stop playing the particle effect.
    public void StopSpecificParticle(Modifier modifier)
    {
        if (_activeModifierParticles.ContainsKey(modifier))
        {
            _activeModifierParticles[modifier].StopAllParticles();
            _activeModifierParticles.Remove(modifier);
        }
        
    }
    
}
