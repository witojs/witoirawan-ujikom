using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private GameObject _deer;
    [SerializeField] private GameObject _dog;
    [SerializeField] private GameObject _horse;
    [SerializeField] private GameObject _cow;
    [SerializeField] private TextMeshProUGUI _timerText;
    
    private float _deerSpeed = 25;
    private float _dogSpeed = 300;
    private float _horseSpeed = 300;
    private float _cowSpeed = 150;
    private float intervalTime = 2f;
    private float timer = 0f;
    private float gameTimer = 0f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameTimer += Time.deltaTime;
        _timerText.SetText($"{gameTimer}");
        if (timer >= intervalTime)
        {
            SpawnEnemy();
            timer = 0;
        }
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     SpawnEnemy();
        // }
    }

    private void SpawnEnemy()
    {
        var enemy = Instantiate(_deer, _enemySpawnPoint.position, _enemySpawnPoint.rotation);
        // enemy.linearVelocity = transform.TransformDirection(Vector3.forward * _deerSpeed * Time.deltaTime);
        // enemy.transform.Translate(Vector3.forward * _deerSpeed * Time.deltaTime);
        enemy.GetComponent<Rigidbody>().linearVelocity = _enemySpawnPoint.transform.forward * _deerSpeed;
        Destroy(enemy, 2f);
    }
}
