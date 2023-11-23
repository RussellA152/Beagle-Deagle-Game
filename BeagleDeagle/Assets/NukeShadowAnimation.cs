using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeShadowAnimation : MonoBehaviour
{
    private Vector3 _startingSize;
    public Vector3 endSize = new Vector3(2f, 2f, 2f);
    public float lerpDuration = 2f;

    private void Awake()
    {
        _startingSize = transform.localScale;
    }

    private void OnEnable()
    {
        // Make nuke shadow get larger with time
        LeanTween.scale(gameObject, endSize, lerpDuration).setEase(LeanTweenType.easeOutQuad);
        
    }

    private void OnDisable()
    {
        // Reset nuke shadow size
        transform.localScale = _startingSize;
    }
}
