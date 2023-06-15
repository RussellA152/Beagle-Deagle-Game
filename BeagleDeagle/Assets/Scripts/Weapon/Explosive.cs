using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Explosive<T> : MonoBehaviour where T: ScriptableObject
{
    [SerializeField]
    protected GameObject sprite;

    [SerializeField]
    protected T explosiveData;

    [SerializeField]
    protected AreaOfEffect areaOfEffect;

    private void OnDisable()
    {
        areaOfEffect.gameObject.SetActive(false);

        sprite.SetActive(true);

        StopAllCoroutines();
    }


    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public abstract IEnumerator Detonate();

    public virtual void UpdateExplosiveData(T scriptableObject)
    {
        explosiveData = scriptableObject;
    }
}
