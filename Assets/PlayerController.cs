using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] private GameObject _foodPrefab;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private float _horizontalInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetTrigger("strafe-left");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetTrigger("strafe-right");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("throw");
            ThrowFood();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(_horizontalInput * _speed, _rigidbody.linearVelocity.y, 0f);
    }

    private void ThrowFood()
    {
        var food = Instantiate(_foodPrefab, _spawnPoint.position, _spawnPoint.rotation);
        // food.GetComponent<Rigidbody>().linearVelocity = transform.TransformDirection(Vector3.forward * 300 * Time.deltaTime);
        food.GetComponent<Rigidbody>().linearVelocity = _spawnPoint.transform.forward * 10;
        // food.GetComponent<Rigidbody>().linearVelocity = _spawnPoint.forward * 300;
        Destroy(food, 3f);
    }
}
