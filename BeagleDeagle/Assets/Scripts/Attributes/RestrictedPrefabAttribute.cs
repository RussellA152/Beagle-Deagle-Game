using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class RestrictedPrefabAttribute : PropertyAttribute
{
    public Type ComponentType { get; private set; }

    public RestrictedPrefabAttribute(Type componentType)
    {
        ComponentType = componentType;
    }
}