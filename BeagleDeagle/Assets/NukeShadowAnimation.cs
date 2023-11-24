using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeShadowAnimation : MonoBehaviour
{
    private Vector3 _startingSize;
    [SerializeField] private Vector3 endSize = new Vector3(2f, 2f, 2f);

    private Nuke _nukeScript;
    private void Awake()
    {
        // Cache how big the nuke's shadow is so we can reset on disable
        _startingSize = transform.localScale;
        
        _nukeScript = GetComponentInParent<Nuke>();
    }

    private void OnDisable()
    {
        // Reset nuke shadow size
        transform.localScale = _startingSize;
    }

    public void PlayNukeShadowAnimation()
    {
        // Make nuke shadow get larger with time (will use detonation time)
        LeanTween.scale(gameObject, endSize, _nukeScript.GetDetonationTime()).setEase(LeanTweenType.easeOutQuad);
    }
}
