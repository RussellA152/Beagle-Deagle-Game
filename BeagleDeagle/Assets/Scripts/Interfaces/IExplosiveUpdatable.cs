using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosiveUpdatable : IDataUpdatable<ExplosiveData>
{
    ///-///////////////////////////////////////////////////////////
    /// Start the detonation timer for an explosive. Use the vector2
    /// parameter for giving the explosive a trajectory.
    /// 
    public void Activate(Vector2 aimDirection);

    ///-///////////////////////////////////////////////////////////
    /// Set the damage for the explosive.
    /// 
    public void SetDamage(float explosiveDamage);

    ///-///////////////////////////////////////////////////////////
    /// Set the duration for the explosive.
    /// 
    public void SetDuration(float explosiveDuration);
}
