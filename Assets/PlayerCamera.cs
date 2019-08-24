using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f; 
    
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;

    private void FixedUpdate()
    {
//        Vector3 desiredPosition = target.position + offset;
//        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
//        transform.position = smoothedPosition;
    }

    private void Update()
    {
        transform.Translate(new Vector3(0f, _moveSpeed * Time.deltaTime, 0f));
    }
}
