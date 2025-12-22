using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] private GameObject _foodPrefab;
    private Animator _animator;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // float horizontalInput = Input.GetAxis("Horizontal");
        // //get the Input from Vertical axis
        // float verticalInput = Input.GetAxis("Vertical");
        // transform.position = transform.position + new Vector3(horizontalInput * _speed * Time.deltaTime, verticalInput * _speed * Time.deltaTime, 0);

        if (Input.GetKeyDown(KeyCode.A))
        {
            // _animator.SetBool("isLeft", true);
            _animator.SetTrigger("strafe-left");
            gameObject.transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetTrigger("strafe-right");
            gameObject.transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("throw");
            ThrowFood();
        }
    }
    
    private void ThrowFood()
    {
        var food = Instantiate(_foodPrefab, _spawnPoint.position, _spawnPoint.rotation);
        food.GetComponent<Rigidbody>().linearVelocity = transform.TransformDirection(Vector3.forward * 300 * Time.deltaTime);
        Destroy(food, 3f);
    }
}
