using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletUpdatable : IDataUpdatable<BulletData>
{
    ///-///////////////////////////////////////////////////////////
    /// Update the damage and penetration values of this bullet.
    /// This is often done by a gun or enemy that uses a bullet.
    /// 
    public void UpdateDamageAndPenetrationValues(float damage, int penetration);

    ///-///////////////////////////////////////////////////////////
    /// Tell the bullet the object that shot it.
    /// 
    public void UpdateWhoShotThisBullet(Transform shooter);
}
