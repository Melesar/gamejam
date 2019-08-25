using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;

    public void SwitchZ(float vertical)
    {
        offset.z = Mathf.Sign(vertical) * Mathf.Abs(offset.z);
    }

    public void SwitchX()
    {
        offset.x *= -1;
    }
    
    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
