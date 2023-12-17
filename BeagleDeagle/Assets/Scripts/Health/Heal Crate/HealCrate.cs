using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCrate : MonoBehaviour
{
    private IHealth _healthScript;
    private bool _destroyed;

    [SerializeField] private GameObject crateGameObject;
    [SerializeField] private GameObject healItemGameObject;

    private void Awake()
    {
        _healthScript = crateGameObject.GetComponent<IHealth>();
    }

    private void Start()
    {
        crateGameObject.SetActive(true);
        healItemGameObject.SetActive(false);
    }

    private void Update()
    {
        _destroyed = _healthScript.IsDead();

        if (_destroyed)
        {
            crateGameObject.SetActive(false);
            healItemGameObject.SetActive(true);
        }
    }
}
