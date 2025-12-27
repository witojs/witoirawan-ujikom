using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _score;

    public void UpdateScore()
    {
        _scoreText.text = $"Score: {_score}";
    }

    public void AddScore(int score)
    {
        _score += score;
        UpdateScore();
    }
}
