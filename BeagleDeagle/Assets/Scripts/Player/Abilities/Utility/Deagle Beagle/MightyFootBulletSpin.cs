using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightyFootBulletSpin : MightyFootBullet
{
    protected override void ApplyTrajectory()
    {
        //rb.velocity = transform.right * bulletData.bulletSpeed;

        // Rotate the object 360 degrees continuously
        LeanTween.rotate(gameObject, Vector3.forward * 360f, bulletData.GetLifeTime())
            .setEase(LeanTweenType.easeInOutQuad) // You can change the ease type
            .setLoopClamp(); // This will make the rotation continuous
        
        
    }
}

