using System;
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;

public class PoolableParticle : MonoBehaviour, IPoolable
{
    [SerializeField] private int poolKey;
    public int PoolKey => poolKey;

    public ParticleSystem[] particleSystemGameObjects;

    private Vector3 _originalSize;

    [SerializeField] private Transform originalParent;
    [SerializeField] private Transform currentParent;
    
    

    private void Awake()
    {
        foreach (ParticleSystem particleSys in particleSystemGameObjects)
        {
            particleSys.gameObject.SetActive(false);
        }

        _originalSize = particleSystemGameObjects[0].transform.localScale;
    }

    private void OnEnable()
    {
        originalParent = transform.parent;
    }

    private void OnDisable()
    {
        if (originalParent != null)
        {
            transform.SetParent(originalParent);
        }
        
        foreach (ParticleSystem particleSys in particleSystemGameObjects)
        {
            particleSys.gameObject.SetActive(false);
            particleSys.transform.localScale = _originalSize;
        }
        
    }

    ///-///////////////////////////////////////////////////////////
    /// Set all particle system gameObjects active, most of them play on awake.
    /// 
    public void PlayAllParticles(float sizeMultiplier)
    {
        foreach (ParticleSystem particleSys in particleSystemGameObjects)
        {
            var localScale = particleSys.transform.localScale;
            
            localScale = new Vector3(sizeMultiplier *localScale.x ,
                sizeMultiplier * localScale.y,sizeMultiplier * localScale.z);
            
            particleSys.transform.localScale = localScale;
            
            particleSys.gameObject.SetActive(true);
        }
        

    }

    ///-///////////////////////////////////////////////////////////
    /// Tell all particle systems in this gameObject to stop playing
    /// 
    public void StopAllParticles()
    {
        foreach (ParticleSystem particleSys in particleSystemGameObjects)
        {
            particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            
            CFXR_Effect effect = particleSys.GetComponent<CFXR_Effect>();
            
            if(effect != null)
                effect.DisableParticle();

        }
    }

    public void PlaceParticleOnTransform(Transform transformToPlaceAt)
    {
        transform.position = transformToPlaceAt.position;
        gameObject.SetActive(true);
    }

    public void StickParticleToTransform(Transform transformToStickTo)
    {
        Transform transformComponent = transform;
        
        transformComponent.position = transformToStickTo.position;

        gameObject.SetActive(true);
        
        transformComponent.SetParent(transformToStickTo);
        
        currentParent = transformComponent.parent;
    }
    
}
