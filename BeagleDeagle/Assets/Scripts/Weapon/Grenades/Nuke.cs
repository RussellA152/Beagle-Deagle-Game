using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Explosive
{
    private bool _explosionHappening = false;
    
    public override void Activate(Vector2 aimDirection)
    {
        base.Activate(aimDirection);
        
        transform.position = aimDirection;
        
        StartCoroutine(Detonate());
    }
    

    protected override void AfterDetonation()
    {
        base.AfterDetonation();
        
        sprite.SetActive(false);

        StartCoroutine(BrieflyShowGizmo());
    }

    protected override void PlayActivationSound()
    {
        AudioClipPlayer.PlayForDurationAudioClip(ExplosiveData.activationSound,ExplosiveData.explosiveSoundVolume, ExplosiveData.detonationTime);
    }


    private void OnDrawGizmos()
    {
        if (_explosionHappening)
        {
            Gizmos.color = Color.red;
        
            Gizmos.DrawWireSphere(transform.position, ExplosiveData.explosiveRadius);
        }
    }
    
    private IEnumerator BrieflyShowGizmo()
    {
        _explosionHappening = true;
        yield return new WaitForSeconds(1.5f);
        _explosionHappening = false;
    }
    
}
