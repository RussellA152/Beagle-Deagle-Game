using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedLine : MonoBehaviour {

    private SpriteRenderer rend;
    private LineRenderer line;

	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        line.material.mainTexture = rend.sprite.texture;
	}
}
