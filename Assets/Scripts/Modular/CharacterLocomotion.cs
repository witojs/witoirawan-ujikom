using System;
using UnityEngine;

// handle math for speed, smoothing, movement
public class CharacterLocomotion : MonoBehaviour
{
    [SerializeField] private MovementData _stats;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody _rb;
    private Animator _anim;
    private float _currentSpeed;
    private float _speedVelocity;
    private float _coyoteTimeCounter;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        ApplyJumpGravity();
    }

    public void Move(Vector3 direction, bool isSprinting)
    {
        // normalized so the speed is consistent
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        
        float targetSpeed = direction.sqrMagnitude > 0.1f ? 
            (isSprinting ? _stats.sprintSpeed : _stats.walkSpeed) : 0f;
        
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedVelocity, _stats.accelerationTime);
        
        // set air control when jumping
        float currentControl = IsGrounded() ? 1f : _stats.airControl;
        
        Vector3 horizontalVelocity = direction * (_currentSpeed * currentControl);
        
        _rb.linearVelocity = new Vector3(horizontalVelocity.x, _rb.linearVelocity.y, horizontalVelocity.z);

        // Update Animator (Syncing with sprintSpeed from the data asset)
        float animValue = (_currentSpeed / _stats.sprintSpeed) * 2f;
        _anim.SetFloat("MovementSpeed", animValue, 0.05f, Time.deltaTime);

        // set rotation
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _stats.rotationSpeed * Time.fixedDeltaTime);
        }
        
        if (IsGrounded())
        {
            _coyoteTimeCounter = _stats.coyoteTime; // Reset timer while on ground
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime; // Count down when in air
        }
        
        // Tell the animator if we are on the ground or in the air
        _anim.SetBool("isGrounded", IsGrounded());
    }
    
    public void Jump()
    {
        if (_coyoteTimeCounter > 0f)
        {
            // Apply upward force. ForceMode.Impulse is best for instant actions like jumping.
            _rb.AddForce(Vector3.up * _stats.jumpForce, ForceMode.Impulse);
            
            // Trigger the animation
            _anim.SetTrigger("Jump");
            
            _coyoteTimeCounter = 0f;
        }
    }

    public void OnJumpReleased()
    {
        if (_rb.linearVelocity.y > 0)
        {
            _rb.linearVelocity = new Vector3(
                _rb.linearVelocity.x,
                _rb.linearVelocity.y / _stats.lowjumpMultiplier,
                _rb.linearVelocity.z
            );
        }
    }
    
    public bool IsGrounded()
    {
        // Shoot a tiny invisible ray downwards to check for the floor
        float rayLength = 0.2f; 
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, rayLength, _groundLayer);
    }
    
    private void ApplyJumpGravity()
    {
        // 1. If we are falling (moving downwards)
        if (_rb.linearVelocity.y < 0)
        {
            // Apply extra gravity to pull the player down faster
            // Physics.gravity.y is typically -9.81
            // gravity is 1 so fallMultiplier - 1 = desired value (2.5f)
            _rb.linearVelocity += Vector3.up * Physics.gravity.y * (_stats.fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // 2. If we are moving UP but NOT holding the jump button (Short Hop)
        else if (_rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Apply extra gravity to stop the upward momentum quickly
            _rb.linearVelocity += Vector3.up * Physics.gravity.y * (_stats.lowjumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}
