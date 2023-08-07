using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Multiple items/objects in the game might need to update their data
// For example, a weapon could change its data from level 1 data to level 2 data
// Or, an enemy might change its data to go from a regular zombie, to a slow fat zombie.
public interface IDataUpdatable<T> where T: ScriptableObject
{
    ///-///////////////////////////////////////////////////////////
    /// Give a new scriptable object to whatever may need it.
    /// For example, a gun, explosive, or character like a player or enemy may need new
    /// data during gameplay.
    public void UpdateScriptableObject(T scriptableObject);
    
}
