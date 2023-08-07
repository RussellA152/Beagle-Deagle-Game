using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGunDataUpdatable : IDataUpdatable<GunData>
{
    ///-///////////////////////////////////////////////////////////
    /// Return the current data that the player's gun uses
    /// 
    public GunData GetCurrentData();
}
