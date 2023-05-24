using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float GetCurrentHealth();

    //public float GetMaxHealth();

    // add or subtract from health count
    public void ModifyHealth(float amount);

    //public void ModifyMaxHealth(float amount);

    // do something when this entity dies
    public bool IsDead();
}
