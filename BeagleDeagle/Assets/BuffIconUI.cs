using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    [SerializeField] private Image durationFill;
    private float _durationLeft;
    private float _startingDuration;

    private void OnEnable()
    {
        durationFill.fillAmount = 0;
    }
    
    private void Update()
    {
        // If this buff icon has a duration for it, then start adjusting the fill amount 
        if (_startingDuration > 0)
        {
            _durationLeft -= Time.deltaTime;
        
            float fillAmount = Mathf.Clamp01(1 - _durationLeft / _startingDuration);
    
            durationFill.fillAmount = fillAmount;
        }
        
        
    }

    public void UpdateIcon(Sprite newSprite)
    {
        icon.sprite = newSprite;
    }

    public void SetBuffTimer(float timer)
    {
        _startingDuration = timer;
        _durationLeft = timer;
        
    }
}
