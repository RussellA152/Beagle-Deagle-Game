using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponType : ScriptableObject
{
    public List<StatusEffectData> statusEffects = new List<StatusEffectData>();
        
    ///-///////////////////////////////////////////////////////////
    /// Return the first instance of a specific status effect type, inside of the list of status effects
    /// 
    public T GetStatusEffectData<T>()
    {
        return statusEffects.OfType<T>().FirstOrDefault();
    }
}
