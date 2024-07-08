using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : MonoBehaviour
{
    [Header("Turrets")]
    public GameObject[] turrets;
    public GameObject waves;
    public Player player;

    public bool protectActive = false;
    public GameManager gameManager;

    [Header("Health")]
    public float health;
    public float maxHealth;

    public void OnEnable()
    {
        health = maxHealth;
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void Update()
    {
        // lets every turret turn on and off based on the amount of health the wall has
        if (health <= (maxHealth - 100))
            turrets[0].SetActive(false);

        else
            turrets[0].SetActive(true);

        if (health <= (maxHealth - 200))
            turrets[1].SetActive(false);

        else
            turrets[1].SetActive(true);

        if (health <= (maxHealth - 300))
            turrets[2].SetActive(false);

        else
            turrets[2].SetActive(true);

        if (health <= (maxHealth - 400))
            turrets[3].SetActive(false);

        else
            turrets[3].SetActive(true);

        if (health <= (maxHealth - 500))
            turrets[4].SetActive(false);

        else
            turrets[4].SetActive(true);
    }

    // the wall takes damage
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
            Die();
    }

    // the objective ends
    void Die()
    {
        player.Die();
        waves.SetActive(false);
    }
}
