using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimation : MonoBehaviour {

    public Vector2 speed;
    private Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        if (rend == null) { Destroy(this); }
	}
	
	// Update is called once per frame
	void Update () {
        rend.material.mainTextureOffset += speed * Time.deltaTime;
	}
}
