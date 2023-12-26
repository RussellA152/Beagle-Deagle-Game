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
        yield return new WaitForSeconds(0.5f);
        _explosionHappening = false;
    }
    
}
