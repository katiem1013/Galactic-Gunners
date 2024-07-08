using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public GameManager gameManager;
    static public bool objectiveCompleted;

    public GameObject enemySpawner;
   
    [Header("Waves")]
    [SerializeField] private float countdown;
    [SerializeField] public Wave[] waves;
    public int currentWaveIndex = 0;
    public float spawnRange;
    public List<GameObject> currentEnemy;

    public Transform spawner;

    private void OnEnable()
    {
        GameManager.escortDeaths = 0;
        GameManager.escortKills = 0;
        currentWaveIndex = 0;
        enemySpawner.SetActive(false);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in gos)
            Destroy(go);

    }

    private void OnDisable()
    {
        enemySpawner.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager.currentObjective = "Protect";
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (currentWaveIndex >= waves.Length)
        {
            if (!objectiveCompleted)
                GameManager.completedObjectives++; // adds to the amount of completed objectives

            objectiveCompleted = true;
            gameObject.SetActive(false);
        }

        if(currentEnemy.Count == 0)
        {
            currentWaveIndex++;
            SpawnWave();
        }

        if (currentEnemy[0] == null)
            currentEnemy.Remove(currentEnemy[0]);
    }

    void SpawnWave()
    {
        if (currentWaveIndex < waves.Length)
            for (int i = 0; i < waves[currentWaveIndex].GetMonsterSpawnList().Length; i++)
            {
                GameObject enemy = Instantiate(waves[currentWaveIndex].enemies[i], spawner.transform.position, Quaternion.identity);
                enemy.SetActive(true);
                enemy.transform.parent = spawner;
                currentEnemy.Add(enemy);

                Target target = enemy.GetComponent<Target>();
                target.SetSpawner(this);
            }

    }

    [System.Serializable]
    public class Wave
    {
        [SerializeField] public GameObject[] enemies;
        public GameObject[] GetMonsterSpawnList()
        {
            return enemies;
        }
    }
}
