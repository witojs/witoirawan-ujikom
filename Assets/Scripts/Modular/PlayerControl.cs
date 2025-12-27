using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private CharacterLocomotion _locomotion;
    private float _hInput, _vInput;
    
    [Header("Input Setting")]
    [SerializeField] private float _jumpBufferTime = 0.2f;
    private float _jumpBufferCounter;

    void Start() => _locomotion = GetComponent<CharacterLocomotion>();

    void Update()
    {
        _hInput = Input.GetAxis("Horizontal");
        _vInput = Input.GetAxis("Vertical");

        // Action logic (Space for Throwing) stays here or in a separate script
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            // Trigger throw logic
        }
        
        // 1. Initial Jump Press
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        
        if (_jumpBufferCounter > 0f)
        {
            // We call the locomotion Jump. It returns 'true' if the jump actually happened
            if (_locomotion.Jump())
            {
                _jumpBufferCounter = 0f; // Clear the buffer so we don't jump twice
            }
        }

        // 2. Detect Button Release
        if (Input.GetButtonUp("Jump"))
        {
            _locomotion.OnJumpReleased();
        }
    }

    void FixedUpdate()
    {
        // Calculate camera-relative direction
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 moveDir = (camForward * _vInput) + (Camera.main.transform.right * _hInput);
        
        // Tell the locomotion component to move!
        _locomotion.Move(moveDir, Input.GetKey(KeyCode.LeftShift));
    }
}
