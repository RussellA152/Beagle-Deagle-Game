using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeSizeIncrease : MonoBehaviour
{
    private Vector3 _startSize;
    public Vector3 endSize = new Vector3(2f, 2f, 2f);
    public float lerpDuration = 2f;

    private float elapsedTime = 0f;

    private void Start()
    {
        _startSize = transform.localScale;
    }

    void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the lerp factor (0 to 1)
        float lerpFactor = Mathf.Clamp01(elapsedTime / lerpDuration);

        // Lerp between the start and end sizes
        Vector3 lerpedSize = Vector3.Lerp(_startSize, endSize, lerpFactor);

        // Apply the lerped size to the object's scale
        transform.localScale = lerpedSize;

        // Optionally, reset the timer if the lerp is complete
        // if (lerpFactor >= 1f)
        // {
        //     elapsedTime = 0f;
        // }
    }
}
