using System;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _playerTransform;

    [Header("Settings")]
    [SerializeField] private float _distance = 7.0f;
    [SerializeField] private float _sensitivity = 3.0f;
    [SerializeField] private float _yMinLimit = -20f;
    [SerializeField] private float _yMaxLimit = 80f;
    
    private float _currentX = 0.0f;
    private float _currentY = 0.0f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (_playerTransform == null) return;
        
        //get mouse input
        _currentX += Input.GetAxis("Mouse X") * _sensitivity;
        _currentY += Input.GetAxis("Mouse Y") * _sensitivity;
        
        //clamp the vertical rotation to avoid camera flip upside down
        _currentY  = Mathf.Clamp(_currentY, _yMinLimit, _yMaxLimit);
        
        //convert rotation to Quaternion based on mouse move
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0.0f);
        
        //calculate new position
        //take the rotation, multiply by backward vector, add player position
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -_distance);
        //add player transform position so the orbit center stays locked to wherever player walks
        Vector3 position = rotation * negDistance + _playerTransform.position;
        
        transform.position = position;
        transform.rotation = rotation;
    }
}
