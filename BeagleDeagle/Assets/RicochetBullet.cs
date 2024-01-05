using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : Bullet<RicochetBulletData>
{
    private Camera mainCamera;
    
    private bool hasReflected = false;

    // Bullet will automatically disable when it reflects too many times (depends on scriptable object)
    private int _timesReflected;
    
    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        // Reset amount of times reflected;
        _timesReflected = 0;
    }

    void Update()
    {
        CheckScreenBoundaries();
    }

    void CheckScreenBoundaries()
    {
        // TODO: Make this use camera boundaries instead!
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);

        if ((viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) && !hasReflected)
        {
            // When bullet goes outside of the screen boundaries, reflect its velocity
            Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, screenPos.z);

            Vector2 reflectDirection = (screenCenter - screenPos).normalized;
            rb.velocity = Vector2.Reflect(rb.velocity, reflectDirection);

            // Bullet has successfully reflected
            hasReflected = true;
            _timesReflected++;
        }

        // Don't allow the bullet to reflect again in the same frame
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            hasReflected = false;
        }
        
        // Disable bullet once it has reached its ricochet limit
        if(_timesReflected >= bulletData.ricochetLimit)
            gameObject.SetActive(false);
    }

}
