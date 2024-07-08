using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public enemySpawner enemy;

    [Header("Resistences and Weakness")]
    public string equippedGun;
    public string resistance, weakness;
    public WeaponSwitching weaponSwitching;

    [Header("Objectives")]
    public Waves Spawner;
    public GameManager gameManager;
    public bool canDie = true;

    [Header("Audio")]
    public AudioClip[] deathSounds;
    public AudioSource audioSource;
    public float soundVolume;

    private void Start()
    {
        enemySpawner enemy = GetComponent<enemySpawner>();
    }

    private void Update()
    {
        equippedGun = weaponSwitching.equippedWeapon;

        if (health <= 0f)
        {
            if (canDie)
            {
                if (gameManager.currentObjective == "Payload")
                {
                    canDie = false;
                    GameManager.escortKills++;
                }

                if (gameManager.currentObjective == "Collect")
                {
                    canDie = false;
                    GameManager.collectKills++;
                }

                if (gameManager.currentObjective == "Destroy")
                {
                    canDie = false;
                    GameManager.destroyKills++;
                }

                if (gameManager.currentObjective == "Protect")
                {
                    canDie = false;
                    GameManager.protectKills++;
                }
                else
                    canDie = false;

            }

            else
            {
                Invoke("Die", 0.1f);
            }
        }
            
    }

    public void TakeDamage(float amount)
    {
        if (equippedGun == resistance)
            health -= (amount * 0.75f);

        else if (equippedGun == weakness)
            health -= (amount * 1.25f);

        else
            health -= amount;
    }

    IEnumerator Die()
    {

        PlaySound();

        if (gameManager.currentObjective == "Protect" && Spawner != null)
            Spawner.currentEnemy.Remove(this.gameObject);
        Destroy(gameObject);
        if (!canDie)
        {
            canDie = true;
            enemy.enemyCount -= 1;
        }
        

        return null;
    }
    
    public void SetSpawner(Waves _spawner)
    {
        Spawner = _spawner;
    }

    void PlaySound()
    {
        // chooses a random sound from the list
        int soundChance = Random.Range(1, deathSounds.Length);
        audioSource.clip = deathSounds[soundChance];
        audioSource.PlayOneShot(audioSource.clip); // plays the sound once

        // moves the played sound to index 0 to stop it being repeated
        deathSounds[soundChance] = deathSounds[0];
        deathSounds[0] = audioSource.clip;
    }
}
