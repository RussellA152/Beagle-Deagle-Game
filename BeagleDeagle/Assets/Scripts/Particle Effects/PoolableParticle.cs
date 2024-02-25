using System;
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolableParticle : MonoBehaviour, IPoolable
{
    [SerializeField] private int poolKey;
    public int PoolKey => poolKey;

    public ParticleSystem[] particleSystemGameObjects;
    
    //private Vector3[] _originalSizes;
    private Dictionary<Transform, Vector3> _originalSizes = new Dictionary<Transform, Vector3>();

    private Transform _originalParent;
    private Transform _currentParent;

    /* Set this true if the particle effect should be backwards when sticking to another transform
     Use this for any particles that have text.*/
    [SerializeField] private bool alwaysPositiveScaleOnStick;

    // The movement script of whoever this particle is stuck to
    private IMovable _stuckToMovableScript;

    private void Awake()
    {
        for (int i = 0; i < particleSystemGameObjects.Length; i++)
        {
            Transform currentTransform = particleSystemGameObjects[i].transform;
            _originalSizes[currentTransform] = currentTransform.localScale;
            particleSystemGameObjects[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _originalParent = transform.parent;
    }

    private void OnDisable()
    {
        if (_originalParent != null && gameObject.activeInHierarchy)
        {
            transform.SetParent(_originalParent);
        }
        
        // Set particle effects back to their original sizes
        for (int i = 0; i < particleSystemGameObjects.Length; i++)
        {
            Transform currentTransform = particleSystemGameObjects[i].transform;
            currentTransform.localScale = _originalSizes[currentTransform];
            particleSystemGameObjects[i].gameObject.SetActive(false);
        }
        
        if(_stuckToMovableScript != null)
            _stuckToMovableScript.entityStartedFacingRight -= FixRotation;
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

    ///-///////////////////////////////////////////////////////////
    /// Move particle effect to the position of a transform. If you want to
    /// have the particle effect continuously play on top of a transform, then use the StickParticleToTransform method.
    public void PlaceParticleOnTransform(Transform transformToPlaceAt)
    {
        transform.position = transformToPlaceAt.position;
        gameObject.SetActive(true);
    }

    ///-///////////////////////////////////////////////////////////
    /// Give particle system a new parent transform so it can play its effect on top of that transform.
    /// 
    public void StickParticleToTransform(Transform transformToStickTo)
    {
        Transform transformComponent = transform;

        transformComponent.position = transformToStickTo.position;
        gameObject.SetActive(true);
        transformComponent.SetParent(transformToStickTo);
        _currentParent = transformComponent.parent;

        FixRotation(transformToStickTo.localScale.x > 0);
    }

    public void ChangeRotationDynamically(IMovable movableScript)
    {
        _stuckToMovableScript = movableScript;
        
        _stuckToMovableScript.entityStartedFacingRight += FixRotation;
    }

    private void FixRotation(bool parentTransformFacingRight)
    {
        if (alwaysPositiveScaleOnStick)
        {
            Vector3 localScale = transform.localScale;

            if (parentTransformFacingRight)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }
            else
            {
                localScale.x = Mathf.Abs(localScale.x) * -1f;
            }

            localScale.y = Mathf.Abs(localScale.y);
            localScale.z = Mathf.Abs(localScale.z);

            transform.localScale = localScale;
        }
    }
}
