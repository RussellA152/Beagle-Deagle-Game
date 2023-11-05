using System.Collections.Generic;
using UnityEngine;
public class Modifier
{
    // The name of the object or item that is applying this modifier
    public string modifierName;

    // Is the modifier currently in effect?
    [HideInInspector]
    public bool isActive;
}

