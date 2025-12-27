using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 10f;
    [SerializeField] private float _sprintSpeed = 20f;
    // smoothing duration low value makes characeter feel light, more value character feel heavy
    [SerializeField] private float _accelerationTime = 0.2f;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] private GameObject _foodPrefab;
    [SerializeField] private float _rotationSpeed;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private float _horizontalInput;
    private float _verticalInput;
    private float _currentSpeed;
    private float _speedVelocity; // Required by SmoothDamp to track rate of change

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        
        Vector3 inputDir =  new Vector3(_horizontalInput, 0, _verticalInput);
        // Magnitude is the actual length of the arrow (the distance from (0,0,0) to your input point).
        // using square magnitude much faster for computer as magnitude formula itself use Square root which is expensive
        float inputMagnitude = inputDir.sqrMagnitude;

        // if idle target speed 0, if walk 10, if sprint 20
        float targetSpeed = 0;

        // using 0.1f prevent of drifting as if joystick it usually move a bit
        if (inputMagnitude > 0.1f)
        {
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            targetSpeed = isSprinting ? _sprintSpeed : _walkSpeed;
        }

        // current speed get closer to target speed automatically slows down the acceleration, prevent character start and stops instanly
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedVelocity, _accelerationTime);
        
        // value mapping convert physical speed into animator value
        // if currentSpeed is 10 (10/20) * 2 = 1 animator plays walk anim
        // if current speed is 15 (15/20) * 2 = 1.5 animator plays mix walk and run
        // 2f because blend tree has range 0-2 (0 idle, 1 walk, 2 run)
        float smoothedAnimValue = (_currentSpeed / _sprintSpeed) * 2f;
        
        // add damping (0.05f), Time.deltaTime calculate damping relative to time
        _animator.SetFloat("MovementSpeed", smoothedAnimValue, 0.05f, Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("throw");
            ThrowFood();
        }

    }

    private void FixedUpdate()
    {
        // Camera follow Version
        /*Vector3 direction = new Vector3(_horizontalInput, 0, _verticalInput);

        // Normalize vector agar diagonal tidak lebih cepat, karena jika berjalan vertical & horizontal bersamaan
        // akan berjalan full speed 
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        _rigidbody.linearVelocity = new Vector3(
            direction.x * _speed,
            _rigidbody.linearVelocity.y,
            direction.z * _speed
        );

        //rotate player only if move
        if (direction != Vector3.zero)
        {
            //calculate rotation want to reach, need forwardVector and upVector
            //like tells unity which is forward and which is up, using vector3.up lock the char vertical axis
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

            //gradually rotate
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.fixedDeltaTime);
        }*/
        
        //Camera orbit version
        //get camera forward and right vectors
        /*Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        
        //Flatten them so the player doesn't walk into the ground/sky
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();
        
        //calculate movement direction relative to the camera
        Vector3 movementDirection = (camForward * _verticalInput) + (camRight * _horizontalInput);
        
        //apply velocity and rotation
        _rigidbody.linearVelocity = new Vector3(
            movementDirection.x * _speed, 
            _rigidbody.linearVelocity.y, 
            movementDirection.z * _speed
        );*/
        
        //cinemachine orbital follow
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0; // Keep it flat
        camForward.Normalize();

        Vector3 movementDirection = (camForward * _verticalInput) + (Camera.main.transform.right * _horizontalInput);
        _rigidbody.linearVelocity = new Vector3(movementDirection.x * _currentSpeed, _rigidbody.linearVelocity.y, movementDirection.z * _currentSpeed);
        
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void ThrowFood()
    {
        var food = Instantiate(_foodPrefab, _spawnPoint.position, _spawnPoint.rotation);
        food.GetComponent<Rigidbody>().linearVelocity = transform.forward * 10;
        Destroy(food, 3f);
    }
}