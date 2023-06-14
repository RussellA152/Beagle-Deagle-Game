using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade/Nuclear Bomb")]
public class NuclearBomb : GrenadeData
{
    [SerializeField]
    private UltimateAbilityData ultimateAbilityData;

    public override void Explode()
    {
        // Big explosion hurt all enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(Vector2.zero, areaSpreadX, 1 << LayerMask.NameToLayer("Enemy"));

        Debug.Log("THIS HIT: " + hitEnemies.Length);
    }

    public override float GetDuration()
    {
        // ultimate ability duration
        return ultimateAbilityData.duration;
    }

    public override void OnAreaEnter(Collider2D collision)
    {
        // radiation damage
    }

    public override void OnAreaExit(Collider2D collision)
    {
        // remove radiation?
    }
}
