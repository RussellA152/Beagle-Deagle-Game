using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberZombieController : AIController<BomberEnemyData>
{
    private bool _disableCorpseDuringAnimation = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _disableCorpseDuringAnimation = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    protected override void OnAttack()
    {
        // Don't allow bomber enemies to turn around during their attack
        MovementScript.SetCanFlip(false);
        base.OnAttack();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        if (!AttackScript.AttackIsOnCooldown())
        {
            _disableCorpseDuringAnimation = false;
            StartCoroutine(ExplodeOnDeathAfterDelay());
        }
            

    }

    private IEnumerator ExplodeOnDeathAfterDelay()
    {
        yield return new WaitForSeconds(enemyScriptableObject.explosionOnDeathDelay);
        
        // Spawn an explosion on death
        AttackScript.InitiateAttack();
        
        // Disable corpse after explosion occurs (other enemies do this when death animation completes)
        base.DisableCorpse();
    }

    public override void DisableCorpse()
    {
        if(_disableCorpseDuringAnimation)
            base.DisableCorpse();
    }
}
