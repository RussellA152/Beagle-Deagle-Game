using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private GrenadeData grenadeData;

    [SerializeField]
    private GameObject grenadeSprite;

    [SerializeField]
    private GrenadeAreaOfEffect areaOfEffect;

    [SerializeField]
    private Collider2D grenadeCollider;

    [SerializeField]
    private Rigidbody2D rb;

    private void OnEnable()
    {
        areaOfEffect.UpdateThrowableData(grenadeData);

        StartCoroutine(Detonate());
    }

    private void OnDisable()
    {
        areaOfEffect.gameObject.SetActive(false);
        grenadeSprite.SetActive(true);

        grenadeCollider.enabled = true;

        UnfreezePosition();

        StopAllCoroutines();
    }

    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public IEnumerator Detonate()
    {
        yield return new WaitForSeconds(grenadeData.detonationTime);

        grenadeSprite.SetActive(false);
        areaOfEffect.gameObject.SetActive(true);

        FreezePosition();

        grenadeData.Explode();

        grenadeCollider.enabled = false;


        yield return new WaitForSeconds(grenadeData.duration);

        gameObject.SetActive(false);
        
    }

    public void FreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    }

    public void UnfreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }
    public void UpdateThrowableData(GrenadeData scriptableObject)
    {
        grenadeData = scriptableObject;
    }
}
