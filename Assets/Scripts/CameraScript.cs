using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraScript : MonoBehaviour
{
    private Vector3 cameraOffset;
    private Vector3 offset = new Vector3(0, 2.5f, -3);
    public float smoothTime = 0.3f;
    private Vector3 _currentVelocity = Vector3.zero;
    private Transform mainTarget;
    bool resetMode;


    public void Awake()
    {
        cameraOffset = transform.position;
        Debug.Log(cameraOffset);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition;
        if (mainTarget == null) return;

        if (!resetMode)
        {
            targetPosition = mainTarget.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }
        else
        {
            targetPosition = cameraOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }
    }

    public void UpdateCamera(Transform target)
    {
        resetMode = false;
        mainTarget = target;
    }

    public void ResetCamera()
    {
        resetMode = true;
    }
}
