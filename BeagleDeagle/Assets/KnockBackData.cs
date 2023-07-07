using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKnockBack", menuName = "ScriptableObjects/StatusEffects/KnockBack")]
public class KnockBackData : StatusEffectData
{
    public Vector2 knockBackPower;
}
