using System;
using TMPro;
using UnityEngine;

public class FoodCollision : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _scoreManager.AddScore(1);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    
}
