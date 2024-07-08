using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class enemySpawner : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Enemy Objects")]
    public GameObject nemi;
    public GameObject orzeth;
    public GameObject vylak;
    public GameObject zaryx;
    public GameObject raxor;

    [Header("Enemy Spawn Chances")]
    public float orzethSpawnChance;
    public float vylakSpawnChance;
    public float zaryxSpawnChance;
    public float raxorSpawnChance;

    [Header("Spawn Variable")]
    public int enemyCount;
    public int maxEnemyCount;
    public int xPos, zPos;
    
    void FixedUpdate()
    {
        if (gameManager.currentObjective != "Protect")
            EnemySpawn();
    }

    void EnemySpawn()
    {
        while(enemyCount < maxEnemyCount)
        {
            if (gameManager.currentObjective != "Protect")
            {
                xPos = Random.Range(-35, 35);
                zPos = Random.Range(0, 60);
                Instantiate(nemi, new Vector3(xPos, 0, zPos), Quaternion.identity).SetActive(true);
                enemyCount += 1;

                if (gameManager.currentObjective == "Payload")
                {
                    if (Random.value <= orzethSpawnChance)
                    {
                        xPos = Random.Range(-15, 20);
                        zPos = Random.Range(0, 40);
                        Instantiate(orzeth, new Vector3(xPos, 0, zPos), Quaternion.identity).SetActive(true);
                        enemyCount += 1;
                    }

                    if (Random.value <= vylakSpawnChance)
                    {
                        xPos = Random.Range(-15, 20);
                        zPos = Random.Range(0, 40);
                        Instantiate(vylak, new Vector3(xPos, 0, zPos), Quaternion.identity).SetActive(true);
                        enemyCount += 1;
                    }
                }

                if (gameManager.currentObjective == "Collect")
                {
                    if (Random.value <= zaryxSpawnChance)
                    {
                        xPos = Random.Range(-15, 20);
                        zPos = Random.Range(0, 40);
                        Instantiate(zaryx, new Vector3(xPos, 0, zPos), Quaternion.identity).SetActive(true);
                        enemyCount += 1;
                    }

                    if (Random.value <= raxorSpawnChance)
                    {
                        xPos = Random.Range(-15, 20);
                        zPos = Random.Range(0, 40);
                        Instantiate(raxor, new Vector3(xPos, 0, zPos), Quaternion.identity).SetActive(true);
                        enemyCount += 1;
                    }
                }

                if (gameManager.currentObjective == "Destroy")
                {
                    if (Random.value <= vylakSpawnChance)
                    {
                        xPos = Random.Range(-15, 20);
                        zPos = Random.Range(0, 40);
                        Instantiate(vylak, new Vector3(xPos, 0, zPos), Quaternion.identity).SetActive(true);
                        enemyCount += 1;
                    }

                    if (Random.value <= zaryxSpawnChance)
                    {
                        xPos = Random.Range(-15, 20);
                        zPos = Random.Range(0, 40);
                        Instantiate(zaryx, new Vector3(xPos, 0, zPos), Quaternion.identity).SetActive(true);
                        enemyCount += 1;
                    }

                }
            }
        }

        if (enemyCount > maxEnemyCount)
        {
            maxEnemyCount = 0;
        }

        if (enemyCount <= 20) 
        {
            maxEnemyCount = 30;
        }
    }
}
