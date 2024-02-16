using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public event Action OnGameWon;

    public GameObject enemyPrefab;
    public GameObject healthBarPrefab;
    public int numberOfEnemies = 1;
    public float spawnDelay = 1f;
    private int enemiesAlive = 0;
    public bool gameWon = false;
    public Transform spawnPoint;

    public GameObject winScreen;


    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        StartCoroutine(SpawnEnemies());
        winScreen.SetActive(false);
    }

    void Update()
    {
        if (enemiesAlive <= 0 && !gameWon)
        {
            gameWon = true;
            Debug.Log("Game Won!");
            winScreen.SetActive(true);
            OnGameWon?.Invoke();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy()
    {


        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        GameObject healthBarObject = Instantiate(healthBarPrefab, spawnPoint.position, Quaternion.identity);


        enemiesAlive++;
        Billboard billboard = healthBarObject.GetComponent<Billboard>();
        if (billboard != null)
        {
            billboard.target = enemyObject.transform.GetChild(0).transform;
        }

        Enemy enemy = enemyObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnDeath += EnemyDied;
        }
    }

    private void EnemyDied()
    {
        enemiesAlive--;
    }
}