using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShakeProfile", menuName = "ScriptableObjects/Camera Shake/New Profile")]
public class ScreenShakeData : ScriptableObject
{
    [Header("Impulse Source Settings")] 
    public float impactTime = 0.2f;

    public float impactForce = 1f;

    public float minVelocityX;
    public float maxVelocityX;
    public float minVelocityY;
    public float maxVelocityY;
    
    //public Vector3 defaultVelocity = new Vector3(0, -1f, 0f);
    
    public AnimationCurve impulseCurve;

    [Header("Impulse Listener Settings")] 
    public float listenerAmplitude = 1f;
    public float listenerFrequency = 1f;
    public float listenerDuration = 1f;
}
