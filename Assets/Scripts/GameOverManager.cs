using UnityEngine;
using TMPro;
public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text enemiesKilledText;
    [SerializeField] TMP_Text accuracyText;
    int shotsFired;
    int shotsHit;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        PlayerAttack.instance.OnShoot.AddListener(() => shotsFired++);
        PlayerAttack.instance.OnShootHit.AddListener(() => shotsHit++);
        PlayerAttack.instance.OnPlayerDeath.AddListener(() => ShowGameOver(EnemyManager.instance.enemiesKilled));
    }

    public void ShowGameOver(int enemiesKilled)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Score: {enemiesKilled*100}";
        enemiesKilledText.text = $"Enemies Killed: {enemiesKilled}";
        accuracyText.text = $"Accuracy: {CalculateAccuracy()}%";
    }

    float CalculateAccuracy()
    {
        if (shotsFired == 0) return 0f;
        return (float)shotsHit / shotsFired * 100f;
    }
}
