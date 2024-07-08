using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;
    static public bool objectiveCompleted;

    [Header("Health")]
    public float health = 50f;
    public float maxHealth;
    public float healthRegen;
    public bool takingDamage = false;
    private float time = 0.0f;
    public float regenTime = 1f;

    [Header("Mission Fail")]
    public float targetTime = 600.0f;
    public GameObject destroy, timerObj;
    public Timer timer;

    private void OnEnable()
    {
        health = 225;
        targetTime = 600.0f;
        timerObj.SetActive(true);
        GameManager.destroyDeaths = 0;
        GameManager.destroyKills = 0;
    }

    private void OnDisable()
    {
        timerObj.SetActive(false);
    }

    private void Start()
    {
        maxHealth = health;
        gameManager.currentObjective = "Destroy";
    }

    private void Update()
    {
        timer.timeValue = targetTime;
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            Invoke("MissionFail", 3);
            targetTime = 0.0f;
        }
        
        if (health <= 0f)
            Invoke("Die", 0.1f);

        if (!takingDamage)
            StartHealing();

        if (takingDamage)
            StopCoroutine(WaitForHealing());
    }

    public void TakeDamage(float amount)
    {
        health -= amount; 
        StartCoroutine(WaitForHealing());
    }

    IEnumerator Die()
    {
        if (!objectiveCompleted)
            GameManager.completedObjectives++; // adds to the amount of completed objectives

        objectiveCompleted = true;
        gameObject.SetActive(false);
        return null;
    }

    void MissionFail()
    {
        player.Die(); // sets the player back to spawn
        destroy.SetActive(false); // sets the mission as in active
    }

    void StartHealing()
    {
        time += Time.deltaTime;
        if (time >= regenTime)
        {
            time = time - regenTime;
            if (health < maxHealth)
                health += healthRegen;
        }
    }

    IEnumerator WaitForHealing()
    {
        takingDamage = true;
        yield return new WaitForSeconds(8);
        takingDamage = false;
    }
}
