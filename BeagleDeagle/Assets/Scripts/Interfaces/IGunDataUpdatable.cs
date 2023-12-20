using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGunDataUpdatable : IDataUpdatable<GunData>, IDamager
{
    ///-///////////////////////////////////////////////////////////
    /// Return the current data that the player's gun uses
    /// 
    public GunData GetCurrentData();
    
    public void AddPenetrationModifier(PenetrationModifier modifierToAdd);

    public void RemovePenetrationModifier(PenetrationModifier modifierToRemove);
    public void AddSpreadModifier(SpreadModifier modifierToAdd);

    public void RemoveSpreadModifier(SpreadModifier modifierToRemove);

    public void AddReloadSpeedModifier(ReloadSpeedModifier modifierToAdd);

    public void RemoveReloadSpeedModifier(ReloadSpeedModifier modifierToRemove);

    public void AddAmmoLoadModifier(AmmoLoadModifier modifierToAdd);

    public void RemoveAmmoLoadModifier(AmmoLoadModifier modifierToRemove);
}
