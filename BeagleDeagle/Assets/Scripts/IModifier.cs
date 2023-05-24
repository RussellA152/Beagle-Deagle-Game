using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifier
{
    // Setter Methods

    /// <summary>
    /// Higher values will make entities move faster. Lower values will make entities move slower.
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyMovementSpeedModifier(float amount);

    /// <summary>
    /// Higher values will make entities attack or shoot faster. Lower values will make entities attack or shoot slower
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyAttackSpeedModifier(float amount);

    /// <summary>
    /// Higher values will make entities have a higher max health. Lower values will make entities have a lower max health
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyMaxHealthModifier(float amount);

    /// <summary>
    /// Higher values will make entities deal higher damage. Lower values will make entities deal less damage.
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyDamageModifer(float amount);


    // Getter methods (will be converted from a percentage to a float)
    public float GetMovementSpeedModifier();

    public float GetAttackSpeedModifier();

    public float GetMaxHealthModifier();

    public float GetDamageModifier();
}
