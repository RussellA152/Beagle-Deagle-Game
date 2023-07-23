using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour {

    public GameObject linePrefab;
    public int circleCornerCount = 36;
    public float edgeLineOffset = 0.1f;

    private LineRenderer line;
    private void Start()
    {
        //UpdateOutlinerSize();
    }

    public void UpdateOutlinerSize()
    {

        {
            BoxCollider2D collie = GetComponent<BoxCollider2D>();
            if (collie != null)
            {
                Vector3[] points = new Vector3[4];
                points[0] = new Vector3(0.5f * collie.size.x, 0.5f * collie.size.y);
                points[1] = new Vector3(-0.5f * collie.size.x, 0.5f * collie.size.y);
                points[2] = new Vector3(-0.5f * collie.size.x, -0.5f * collie.size.y);
                points[3] = new Vector3(0.5f * collie.size.x, -0.5f * collie.size.y);
                
                if(line == null)
                    line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                
                line.loop = true;
                line.positionCount = 4;
                line.SetPositions(points);
                line.transform.SetParent(collie.transform);
                line.transform.localPosition = (Vector3)collie.offset;
                line.transform.localRotation = Quaternion.identity;
                line.transform.localScale = Vector3.one;
            }
        }
        {
            CircleCollider2D collie = GetComponent<CircleCollider2D>();
            if (collie != null)
            {
                if(line == null)
                    line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                
                line.loop = true;
                line.positionCount = circleCornerCount;
                for (int i = 0; i < line.positionCount; i++)
                {
                    line.SetPosition(i, new Vector3(Mathf.Sin(-i / (float)circleCornerCount * 2 * Mathf.PI), Mathf.Cos(-i / (float)circleCornerCount * 2 * Mathf.PI), 0) * collie.radius);
                }
                line.transform.SetParent(collie.transform);
                line.transform.localPosition = (Vector3)collie.offset;
                line.transform.localRotation = Quaternion.identity;
                line.transform.localScale = Vector3.one;
            }
        }
        {
            PolygonCollider2D collie = GetComponent<PolygonCollider2D>();
            if (collie != null)
            {
                for (int i = 0; i < collie.pathCount; i++)
                {
                    line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                    line.loop = true;
                    line.positionCount = collie.GetPath(i).Length;
                    for (int j = 0; j < collie.GetPath(i).Length; j++)
                    {
                        line.SetPosition(j, new Vector3(collie.GetPath(i)[j].x, collie.GetPath(i)[j].y, 0));
                    }

                    line.transform.SetParent(collie.transform);
                    line.transform.localPosition = (Vector3)collie.offset;
                    line.transform.localRotation = Quaternion.identity;
                    line.transform.localScale = Vector3.one;
                }
            }
        }
        {
            EdgeCollider2D collie = GetComponent<EdgeCollider2D>();
            if (collie != null)
            {
                Vector3[] points = new Vector3[collie.points.Length];
                
                if(line == null)
                    line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                line.loop = false;
                line.positionCount = collie.points.Length;
                for (int i = 0; i < collie.points.Length; i++)
                {
                    points[i] = new Vector3(collie.points[i].x, collie.points[i].y, 0);
                }
                line.SetPositions(points);
                line.widthMultiplier = line.widthMultiplier + collie.edgeRadius;

                line.transform.SetParent(collie.transform);
                line.transform.localPosition = (Vector3)collie.offset + Vector3.up * edgeLineOffset;
                line.transform.localRotation = Quaternion.identity;
                line.transform.localScale = Vector3.one;
            }
        }
        {
            CapsuleCollider2D collie = GetComponent<CapsuleCollider2D>();
            if (collie != null)
            {

                Vector3[] points = new Vector3[circleCornerCount + 2];
                
                if(line == null)
                    line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                line.loop = true;
                line.positionCount = circleCornerCount + 2;

                // float radius = colliderSize.x;
                // float height = colliderSize.y - radius;
                
                float radius = collie.size.x;
                float height = collie.size.y - radius;
                
                int offset = 1;
                Vector3 dir = Vector3.up;
                if (collie.direction == CapsuleDirection2D.Horizontal)
                {
                    // radius = colliderSize.y;
                    // height = colliderSize.x - radius;
                    
                    radius = collie.size.y;
                    height = collie.size.x - radius;
                    
                    offset = 2;
                    dir = Vector3.right;
                }
                if (height < 0) { height = 0; }
                for (int i = 0; i < circleCornerCount / 2; i++)
                {
                    Vector3 pos = new Vector3(Mathf.Sin(-i / (float)circleCornerCount * 2 * Mathf.PI + offset * Mathf.PI / 2), Mathf.Cos(-i / (float)circleCornerCount * 2 * Mathf.PI + offset * Mathf.PI / 2), 0) * radius / 2;
                    pos += dir * height / 2;
                    line.SetPosition(i, pos);
                }
                line.SetPosition(circleCornerCount / 2, new Vector3(-radius / 2, -height / 2, 0));
                if (collie.direction == CapsuleDirection2D.Horizontal)
                {
                    line.SetPosition(circleCornerCount / 2, new Vector3(-height / 2, radius / 2, 0));
                }
                for (int i = circleCornerCount / 2; i < circleCornerCount; i++)
                {
                    Vector3 pos = new Vector3(Mathf.Sin(-i / (float)circleCornerCount * 2 * Mathf.PI + offset * Mathf.PI / 2), Mathf.Cos(-i / (float)circleCornerCount * 2 * Mathf.PI + offset * Mathf.PI / 2), 0) * radius / 2;
                    pos -= dir * height / 2;
                    line.SetPosition(i + 1, pos);
                }
                line.SetPosition(circleCornerCount + 1, new Vector3(radius / 2, height / 2, 0));
                if (collie.direction == CapsuleDirection2D.Horizontal)
                {
                    line.SetPosition(circleCornerCount + 1, new Vector3(height / 2, -radius / 2, 0));
                }
                line.transform.SetParent(collie.transform);
                line.transform.localPosition = (Vector3)collie.offset;
                line.transform.localRotation = Quaternion.identity;
                line.transform.localScale = Vector3.one;

            }
        }
    }
}
