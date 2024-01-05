using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampUpBullet : Bullet<RampUpBulletData>
{
    private float _rampUpAmount;
    protected override void OnEnable()
    {
        base.OnEnable();

        // Damage will increase by x% of the base damage (ex. 200, 220, 240, 260)
        if(bulletData != null)
            _rampUpAmount = DamagePerHit * bulletData.damageIncreasePerHit;
    }

    protected override void Penetrate()
    {
        base.Penetrate();

        // Increase damage every time this bullet penetrates an enemy
        DamagePerHit += _rampUpAmount;
        
        
    }
}
