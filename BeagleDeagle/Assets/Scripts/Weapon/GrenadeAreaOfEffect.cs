using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAreaOfEffect : MonoBehaviour
{
    private GrenadeData grenadeData;

    [SerializeField]
    public CapsuleCollider2D triggerCollider;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((grenadeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            grenadeData.OnAreaEnter(collision);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((grenadeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            grenadeData.OnAreaExit(collision);

        }
        
    }

    public void UpdateThrowableData(GrenadeData scriptableObject)
    {
        grenadeData = scriptableObject;

        triggerCollider.size = new Vector2(grenadeData.areaSpreadX, grenadeData.areaSpreadY);
    }
}
