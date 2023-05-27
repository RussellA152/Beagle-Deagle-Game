using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private GrenadeData grenadeData;

    private void OnEnable()
    {
        StartCoroutine(Detonate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private IEnumerator Detonate()
    {
        yield return new WaitForSeconds(grenadeData.detonationTime);

        grenadeData.Explode();
    }
    public void UpdateThrowableData(GrenadeData scriptableObject)
    {
       grenadeData = scriptableObject;

    }
}
