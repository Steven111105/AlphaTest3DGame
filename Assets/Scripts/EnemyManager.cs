using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] CharacterStats enemyStats;
    [SerializeField] GameObject enemyPrefab;
    public static EnemyManager instance;
    public int enemiesKilled = 0;
    public UnityEvent OnEnemyKilled;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        OnEnemyKilled.AddListener(HandleOnEnemyKilled);
        StartCoroutine(SpawnEnemy(new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10))));
    }

    void HandleOnEnemyKilled()
    {
        enemiesKilled++;
        Debug.Log("Enemies killed: " + enemiesKilled);
        StartCoroutine(SpawnEnemy(new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10))));
        
    }

    IEnumerator SpawnEnemy(Vector3 position = default)
    {
        yield return new WaitForSeconds(1f);
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.GetComponent<EnemyScript>().LoadStat(enemyStats);
    }
}
