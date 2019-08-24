using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
        transform.position = smoothedPosition;
        
    }
}
