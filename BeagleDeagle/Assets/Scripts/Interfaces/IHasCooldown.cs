using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasCooldown
{
    // Id is used to identify scripts that have cooldowns (only needs to use unique locally to the gameObject)
    int Id { get; set; }
    float CooldownDuration { get; set;  }
}
