using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifier
{
    public void ModifySpeedModifer(float amount);

    public void ModifyMaxHealthModifier(float amount);

    public void ModifyDamageModifer(float amount);
}
