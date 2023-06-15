using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private AreaOfEffectData aoeData;

    [SerializeField]
    public CapsuleCollider2D triggerCollider;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((aoeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            aoeData.OnAreaEnter(collision);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((aoeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            aoeData.OnAreaExit(collision);

        }

    }

    public void UpdateAOEData(AreaOfEffectData scriptableObject)
    {
        aoeData = scriptableObject;

        triggerCollider.size = new Vector2(aoeData.areaSpreadX, aoeData.areaSpreadY);
    }
}
