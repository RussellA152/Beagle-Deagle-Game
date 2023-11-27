using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAnimation : MonoBehaviour
{
    [Header("Throwing Animation")]
    // How quickly will the grenade throw flip perform?
    [SerializeField] private float throwFlipDuration = 0.4f;
    private LTDescr _throwTween;  // Keep a reference to the throw animation tween


    [Header("Bouncing Animation")] 
    // How much does grenade bounce up?
    [SerializeField] private float firstBounceOffset = 0.2f;
    // How much does grenade bounce back down after bouncing up?
    [SerializeField] private float secondBounceOffset = -0.1f;
    // How quickly does the grenade perform both bounces?
    [SerializeField] private float bounceDuration = 0.15f;
    
    public void ThrowAnimation(Vector2 direction)
    {
        // Generate a random angle between 180 and 540 degrees
        float randomAngle = Random.Range(-180f, -540f);
        
        // If player is throwing grenade to their left, make the flipping angle positive
        if (direction.x < 0f)
        {
            randomAngle *= -1f;
        }
        

        // Rotate around the Z-axis to the random angle over a duration of 0.5 seconds
        _throwTween = LeanTween.rotateAroundLocal(gameObject, Vector3.forward, randomAngle, throwFlipDuration).setEase(LeanTweenType.easeInQuad);
    }

    ///-///////////////////////////////////////////////////////////
    /// Stop the grenade's throw animation immediately. This is called when the grenade touches a wall
    /// so that it doesn't perform any odd movements.
    /// 
    public void StopThrowAnimation()
    {
        if (_throwTween != null)
        {
            LeanTween.cancel(_throwTween.id, false);
            _throwTween = null;  // Reset the reference after canceling
        }
    }


    public void BounceOnLandAnimation()
    {

        float newPositionY = transform.position.y + firstBounceOffset;
        LeanTween.moveY(gameObject, newPositionY , bounceDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                // Move back down after the first bounce
                LeanTween.moveY(gameObject, newPositionY + secondBounceOffset, bounceDuration)
                    .setEase(LeanTweenType.easeInQuad);
            });
    }
}
