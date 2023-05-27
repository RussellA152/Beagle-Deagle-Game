using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStatModifier : MonoBehaviour, IEnemyStatModifier
{
    [Header("Health & Movement Speed Modifiers")]
    private float maxHealthModifer;// how much extra max health (%) does the AI have?
    private float movementSpeedModifier;// how much extra speed (%) does the AI have? (lower percentages means the player moves slower than usual)

    [Header("Attack Modifiers")]
    private float damageModifer; // how much extra damage (%) does the AI's attacks do?
    private float attackSpeedModifier; // how much faster or slower (%) can this AI attack? (in terms of cooldowns)

    public void ModifyDamageModifier(float amount)
    {
        damageModifer += amount;
    }

    public void ModifyMaxHealthModifier(float amount)
    {
        maxHealthModifer += amount;
    }

    public void ModifyMovementSpeedModifier(float amount)
    {
        movementSpeedModifier += amount;
    }

    public void ModifyAttackSpeedModifier(float amount)
    {
        attackSpeedModifier += amount;
    }

    public float GetMovementSpeedModifier()
    {
        return movementSpeedModifier;
    }

    public float GetMaxHealthModifier()
    {
        return maxHealthModifer;
    }

    public float GetDamageModifier()
    {
        return damageModifer;
    }

    public float GetAttackSpeedModifier()
    {
        return attackSpeedModifier;
    }

}
