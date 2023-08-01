using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosiveUpdatable : IDataUpdatable<ExplosiveData>
{
    public void Explode();
}
