using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private Rigidbody _deer;
    [SerializeField] private Rigidbody _dog;
    [SerializeField] private Rigidbody _horse;
    [SerializeField] private Rigidbody _cow;
    
    private float _deerSpeed = 250;
    private float _dogSpeed = 300;
    private float _horseSpeed = 300;
    private float _cowSpeed = 150;
    private float intervalTime = 1f;
    private float timer = 0f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // timer += Time.deltaTime;
        // if (timer >= intervalTime)
        // {
        //     SpawnEnemy();
        //     timer = 0;
        // }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Rigidbody enemy = Instantiate(_deer, _enemySpawnPoint.position, _enemySpawnPoint.rotation);
        enemy.linearVelocity = transform.TransformDirection(Vector3.forward * _deerSpeed * Time.deltaTime);
        Destroy(enemy, 1f);
    }
}
