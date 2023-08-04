using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasCooldown
{
    int Id { get; set; }
    float CooldownDuration { get; set;  }
}
