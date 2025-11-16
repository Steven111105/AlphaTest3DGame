using UnityEngine;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    void Start()
    {
        EnemyManager.instance.OnEnemyKilled.AddListener(UpdateScore);
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + (EnemyManager.instance.enemiesKilled * 100).ToString();
        PlayerPrefs.SetInt("HighScore", Mathf.Max(PlayerPrefs.GetInt("HighScore", 0), EnemyManager.instance.enemiesKilled * 100));
    }
}
