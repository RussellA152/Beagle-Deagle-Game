using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : Bullet<RicochetBulletData>
{
    private bool _hasReflected = false;

    // Bullet will automatically disable when it reflects too many times (depends on scriptable object)
    private int _timesReflected;

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
        // Disable bullet once it has reached its ricochet limit
        if(_timesReflected > bulletData.ricochetLimit)
            gameObject.SetActive(false);
        
        if (PlayerCamera.Instance.IsTransformOffCameraView(transform) && !_hasReflected)
        {
            // When bullet goes outside of the screen boundaries, reflect its velocity
            Vector2 screenPos = PlayerCamera.Instance.GetMainCamera().WorldToScreenPoint(transform.position);

            Vector2 screenCenter = PlayerCamera.Instance.GetScreenCenter();

            Vector2 reflectDirection = (screenCenter - screenPos).normalized;
            rb.velocity = Vector2.Reflect(rb.velocity, reflectDirection);

            // Bullet has successfully reflected
            _hasReflected = true;
            _timesReflected++;
        }

        // Don't allow the bullet to reflect again in the same frame
        if (!PlayerCamera.Instance.IsTransformOffCameraView(transform))
        {
            _hasReflected = false;
        }

    }

}
