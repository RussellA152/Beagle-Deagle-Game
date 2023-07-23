using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineThicknessAnimation : MonoBehaviour {

    public float amplitude = 0.1f, speed = 1;
    public bool randomOffset;
    private LineRenderer line;
    private float startWidth;
    private float offset = 0;

	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        startWidth = line.widthMultiplier;
        if (randomOffset) { offset = Random.Range(0, Mathf.PI * 2); }
	}
	
	// Update is called once per frame
	void Update () {
        line.widthMultiplier = startWidth + Mathf.Sin(speed * Time.time + offset) * amplitude;
	}
}
