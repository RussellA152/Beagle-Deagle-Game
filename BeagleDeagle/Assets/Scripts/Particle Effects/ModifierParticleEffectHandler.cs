using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// Objects that can be affected by modifiers and will have particle effects play on top of them will require this script.
/// 
public class ModifierParticleEffectHandler : MonoBehaviour
{
    // All modifiers currently playing on this object
    private Dictionary<Modifier, PoolableParticle> _activeModifierParticles = new Dictionary<Modifier, PoolableParticle>();

    private void OnDisable()
    {
        _activeModifierParticles.Clear();
    }

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
                _activeModifierParticles.TryAdd(modifier, particleToPlay);
        
                // Stick the particle to this transform (set particle's parent to this transform),
                // otherwise just move it to the transform's position.
                if(stickToObject)
                    particleToPlay.StickParticleToTransform(transform);
                else
                    particleToPlay.PlaceParticleOnTransform(transform);
            
                // Particle effects that play on transforms will * probably * only need a size of 1
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
