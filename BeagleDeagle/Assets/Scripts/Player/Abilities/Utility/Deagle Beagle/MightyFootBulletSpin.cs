using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MightyFootBulletSpin : MightyFootBullet
{
    [SerializeField, Range(1f, 10f)]  
    private float radiusAroundPlayer;
    
    [SerializeField, Range(0.1f, 5f)] 
    private float rotationTime = 1f;
    
    protected override void ApplyTrajectory()
    {
        Transform bulletTransform = transform;
        
        // This will loop every second for however long the mighty foot lasts
        LeanTween.rotateAround(gameObject, Vector3.forward, -360f, rotationTime)
            .setOnUpdate((float val) =>
            {
                // Update the position based on the circular motion
                float angle = val * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radiusAroundPlayer;
                float y = Mathf.Sin(angle) * radiusAroundPlayer;
                
                // Bullet should follow player
                bulletTransform.position = new Vector3(x + _whoShotThisBullet.transform.position.x, y + _whoShotThisBullet.transform.position.y, bulletTransform.position.z);
            })
            .setLoopClamp();

    }
}

