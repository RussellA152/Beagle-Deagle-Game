using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Multiple items/objects in the game might need to update their data
// For example, a weapon could change its data from level 1 data to level 2 data
// Or, an enemy might change its data to go from a regular zombie, to a slow fat zombie.
public interface IDataUpdatable<T>
{
    public void UpdateScriptableObject(T scriptableObject);
}
