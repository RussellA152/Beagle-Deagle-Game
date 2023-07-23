using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlinesForCollision : MonoBehaviour {

    public GameObject linePrefab;
    public int circleCornerCount = 36;
    public float edgeLineOffset;
    public LayerMask collisionLayers;
    private static OutlinesForCollision instance;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        Collider2D[] collies = FindObjectsOfType<Collider2D>();
        foreach (Collider2D collie in collies)
        {
            if (collisionLayers == (collisionLayers | (1 << collie.gameObject.layer)))
            {
                if (collie.gameObject.GetComponent<Outliner>() == null)
                {
                    AddOutlineToCollider(collie, this);
                }
            }
        }
    }

    public static void AddOutlineToCollider(Collider2D collie, OutlinesForCollision config)
    {
        Outliner o = collie.gameObject.AddComponent<Outliner>();
        o.linePrefab = config.linePrefab;
        o.circleCornerCount = config.circleCornerCount;
        o.edgeLineOffset = config.edgeLineOffset;
    }

    public static void AddOutlineToCollider(Collider2D collie)
    {
        AddOutlineToCollider(collie, instance);
    }
}
