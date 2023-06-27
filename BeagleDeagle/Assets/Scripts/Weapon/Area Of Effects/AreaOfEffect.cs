using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private AreaOfEffectData aoeData;

    [SerializeField]
    private CapsuleCollider2D triggerCollider;


    private void OnDestroy()
    {
        // Resetting the size of the trigger collider when destroyed
        triggerCollider.size = new Vector2(1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((aoeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            aoeData.OnAreaEnter(collision.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((aoeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            aoeData.OnAreaExit(collision.gameObject);

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((aoeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            aoeData.OnAreaStay(transform.position, collision.gameObject);

        }
    }

    public void UpdateAOEData(AreaOfEffectData scriptableObject)
    {
        aoeData = scriptableObject;

        triggerCollider.size = new Vector2(aoeData.areaSpreadX, aoeData.areaSpreadY);
    }
}
