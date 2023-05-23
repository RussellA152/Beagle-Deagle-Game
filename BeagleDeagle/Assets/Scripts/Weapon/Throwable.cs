using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField]
    private ThrowableData throwableData;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if ((throwableData.whatActivatesThrowable.value & (1 << collision.gameObject.layer)) > 0)
    //    {
    //        Debug.Log("THROWABLE HIT " + collision.gameObject.name);

    //        throwableData.SpecialAbility(transform.position);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if ((throwableData.whatActivatesThrowable.value & (1 << collision.gameObject.layer)) > 0)
    //    {
    //        Debug.Log("THROWABLE EXIT " + collision.gameObject.name);

    //        throwableData.SpecialAbility(transform.position);
    //    }
    //}

    //public void UpdateThrowableData(ThrowableData scriptableObject)
    //{
    //    throwableData = scriptableObject;

    //}
}
