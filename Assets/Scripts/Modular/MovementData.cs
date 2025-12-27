using UnityEngine;

//Scriptable object for contain movement data
[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementData")]
public class MovementData : ScriptableObject
{
    [Header("Speed Setting")]
    public float walkSpeed = 10f;
    public float sprintSpeed = 15f;
    public float accelerationTime = 0.2f;
    public float rotationSpeed = 720f;

    [Header("Jump Setting")]
    public float jumpForce = 5f;
    [Range(0, 1)] public float airControl = 0.5f;
    public float coyoteTime = 0.2f;
    
    [Header("Gravity Settings")]
    public float fallMultiplier = 2.5f;
    public float lowjumpMultiplier = 2f; // higher = shorter hop
    
}
