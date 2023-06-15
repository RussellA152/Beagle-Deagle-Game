using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade/Nuclear Bomb")]
public class NuclearBomb : GrenadeData
{
    [SerializeField]
    private UltimateAbilityData ultimateAbilityData;

    [Range(0f, 40f)]
    public float explosiveRadius;

    public override void Explode(Vector2 position)
    {
        // Big explosion hurt all enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(position, explosiveRadius, 1 << LayerMask.NameToLayer("Enemy"));

        // Loop through enemies and apply damage to them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<IHealth>().ModifyHealth(-1f * ultimateAbilityData.abilityDamage);
        }

        Debug.Log("THIS HIT: " + hitEnemies.Length);
    }

    public override float GetDuration()
    {
        // ultimate ability duration
        return ultimateAbilityData.duration;
    }

}
