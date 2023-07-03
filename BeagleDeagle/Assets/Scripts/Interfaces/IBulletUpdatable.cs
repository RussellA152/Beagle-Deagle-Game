using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletUpdatable : IDataUpdatable<BulletData>
{
    public void UpdateWeaponValues(float damage, int penetration);
}
