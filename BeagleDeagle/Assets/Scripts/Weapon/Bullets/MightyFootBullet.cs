using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightyFootBullet : Bullet<MightyFootBulletData>
{
    private int _wallLayerMask;
    private CheckObstruction _obstructionScript;
    private CameraShaker _cameraShaker;

    protected override void Awake()
    {
        base.Awake();
        _obstructionScript = GetComponent<CheckObstruction>();
        _cameraShaker = GetComponent<CameraShaker>();
        
        _wallLayerMask = LayerMask.GetMask("Wall");
    }

    protected override void DamageOnHit(GameObject objectHit)
    {
        // If a wall is between the shooter and target, don't let mighty foot damage the target
        if (_obstructionScript.HasObstruction(_whoShotThisBullet.transform.position, objectHit, _wallLayerMask)) 
            return;

        // Make target take damage
        base.DamageOnHit(objectHit);
        
        _cameraShaker.ShakePlayerCamera(bulletData.screenShakeData);
        
    }

    protected override void UpdateBulletSize()
    {
        float rotationZ = transform.eulerAngles.z;
        
        // Mighty Foot should never face backwards, use rotation to ensure it is upright
        if((rotationZ > 90  && rotationZ < 270) || (rotationZ < -90  && rotationZ < -270))
            transform.localScale = new Vector2(bulletData.sizeX, -1f * bulletData.sizeY);
        else
            transform.localScale = new Vector2(bulletData.sizeX, bulletData.sizeY);
    }
}
