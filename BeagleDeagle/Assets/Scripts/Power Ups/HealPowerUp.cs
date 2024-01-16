using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : PowerUp
{
    [SerializeField] private HealEffectData healEffectData;
    protected override void OnPickUp(GameObject receiverGameObject)
    {
        // Heal the object when they collide with this object
        receiverGameObject.GetComponent<IHealth>().ModifyHealth(healEffectData.healAmount);
    }
}
