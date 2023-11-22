using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenWaypoint : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float threshold = 10f;
    private Camera _mainCamera;
    private bool _isIndicatorActive = true;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_isIndicatorActive)
        {
            Vector3 targetDirection = target.transform.position - transform.position;

            float distanceToTarget = targetDirection.magnitude;

            if (distanceToTarget < threshold)
            {
                gameObject.SetActive(false);
                _isIndicatorActive = false;
            }
            else
            {
                Vector3 targetViewportPosition = _mainCamera.WorldToViewportPoint(target.transform.position);

                if (targetViewportPosition.z > 0 && targetViewportPosition.x > 0 && targetViewportPosition.x < 1 &&
                    targetViewportPosition.y > 0 && targetViewportPosition.y < 1)
                {
                    // Target is on screen, hide the indicator
                    gameObject.SetActive(false);
                }
                else
                {
                    // The target is off screen, show the indicator
                    gameObject.SetActive(true);
                    Vector3 screenEdge = _mainCamera.ViewportToWorldPoint(
                        new Vector3(Mathf.Clamp(targetViewportPosition.x, 0.1f, 0.9f), _mainCamera.nearClipPlane));
                    transform.position = new Vector3(screenEdge.x, screenEdge.y, 0);
                    Vector3 direction = target.transform.position - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0,0,angle);

                }
            }
        }
    }
}
