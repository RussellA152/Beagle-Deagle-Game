using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletUpdatable : IDataUpdatable<BulletData>
{
    public void UpdateDamageAndPenetrationValues(float damage, int penetration);

    public void UpdateWhoShotThisBullet(Transform shooter);
}
