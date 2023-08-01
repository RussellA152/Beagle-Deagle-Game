using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosiveUpdatable : IDataUpdatable<ExplosiveData>
{
    public void Activate(Vector2 aimDirection);
    public void SetDamage(float explosiveDamage);

    public void SetDuration(float explosiveDuration);
}
