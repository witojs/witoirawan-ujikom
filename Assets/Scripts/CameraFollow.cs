using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Setting")]
    [SerializeField] private Transform _playerTransform;

    [Header("Offset Position")]
    [SerializeField] private Vector3 _offset = new Vector3(0f, 5f, -7f);

    [SerializeField] private float _smoothSpeed = 0.125f;

    // using LateUpdate runs after all the Updates functions finished
    private void LateUpdate()
    {
        if (_playerTransform == null) return;
        
        // arrange camera position
        Vector3 desiredPosition = _playerTransform.position + _offset;
        
        // smoothly move the camera
        // Lerp like move from point a to point b by certain percentage
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;
        
        // make camera always look to the player
        transform.LookAt(_playerTransform);
    }
}
