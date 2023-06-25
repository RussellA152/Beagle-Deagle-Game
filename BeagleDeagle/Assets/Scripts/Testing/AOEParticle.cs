using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEParticle : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSys;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("layer: " + other.gameObject.layer);
    }

}
