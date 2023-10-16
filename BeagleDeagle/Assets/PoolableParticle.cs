using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableParticle : MonoBehaviour, IPoolable
{
    [SerializeField] private int poolKey;

    public GameObject[] particleSystemGameObjects;

    private Vector3 _originalSize;

    private void Awake()
    {
        foreach (GameObject particleSystemGO in particleSystemGameObjects)
        {
            particleSystemGO.SetActive(false);
        }

        _originalSize = particleSystemGameObjects[0].transform.localScale;
    }

    private void OnDisable()
    {
        foreach (GameObject particleSystemGO in particleSystemGameObjects)
        {
            particleSystemGO.SetActive(false);
            particleSystemGO.transform.localScale = _originalSize;
        }
    }

    public int PoolKey => poolKey;

    public void PlayAllParticles(float sizeMultiplier)
    {
        foreach (GameObject particleSystemGO in particleSystemGameObjects)
        {
            var localScale = particleSystemGO.transform.localScale;
            
            localScale = new Vector3(sizeMultiplier *localScale.x ,
                sizeMultiplier * localScale.y,sizeMultiplier * localScale.z);
            
            particleSystemGO.transform.localScale = localScale;
            
            particleSystemGO.SetActive(true);
        }
        

    }
    
}
