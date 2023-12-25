using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightyFootBullet : Bullet<MightyFootBulletData>
{
    private int _wallLayerMask;
    private CheckObstruction _obstructionScript;

    protected override void Awake()
    {
        base.Awake();
        _obstructionScript = GetComponent<CheckObstruction>();
    }

    private void Start()
     {
         _wallLayerMask = LayerMask.GetMask("Wall");
     }
    
    protected override void DamageOnHit(GameObject objectHit)
    {
        if (!_obstructionScript.HasObstruction(_whoShotThisBullet.transform.position, objectHit, _wallLayerMask)) 
            return;
        
        // Make target take damage
        base.DamageOnHit(objectHit);
        
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
