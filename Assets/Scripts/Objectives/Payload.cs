using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Payload : MonoBehaviour
{
    public bool payloadActive = false;
    public float enemiesOnPayload = 0;
    public GameManager gameManager;
    public Player player;

    static public bool objectiveCompleted;

    [Header("Health")]
    public float health;
    public float maxHealth;
    public float healthRegen;
    public bool beingAttacked = false;
    private float time = 0.0f;
    public float regenTime = 1f;

    [Header("Movement")]
    public Transform[] path;
    public float speed = 5;
    public float reachDist = 1;
    public int currentWayPoint = 0;
    public bool isMoving;
    public bool enemyOnPayload;
    public Gun gun;
    public WeaponSwitching weaponSwitching;

    [Header("Rotation")]
    public float rotationSpeed = 1;
    private Coroutine LookCoroutine;

    [Header("Mission Fail")]
    public float targetTime = 900.0f;
    public GameObject payload, timerObj;
    public Timer timer;

    public List<GameObject> enemies;
    public Vector3 startingPos;


    // Start is called before the first frame update
    void Start()
    {
        // sets the starting variables
        maxHealth = health;
        gameManager.currentObjective = "Payload";
        currentWayPoint = 0;
    }

    // sets the payload as active when the object is 
    private void OnEnable()
    {
        payloadActive = true;
        timerObj.SetActive(true);
        targetTime = 900.0f;
        GameManager.escortDeaths = 0;
        GameManager.escortKills = 0;
        startingPos = transform.position;
        health = maxHealth;
        currentWayPoint = 0;
    }

    // sets the payload as not active when the object is
    private void OnDisable()
    {
        payloadActive = false;
        timerObj.SetActive(false);
        gameObject.transform.position = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        enemiesOnPayload = enemies.Count;

        timer.timeValue = targetTime;

        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            Invoke("MissionFail", 3);
        }

        gun = weaponSwitching.GetComponentInChildren<Gun>(); // gets the weapon switching

        // gets the distance to the next waypoint
        float dist = Vector3.Distance(path[currentWayPoint].position, transform.position);

        if (enemiesOnPayload > 0)
            isMoving = false; // sets the payload as stationary

        if (enemiesOnPayload == 0)
        {
            // sets the payload as moving 
            isMoving = true;

            // lets the payload heal when no enemies are on payload
            StartHealing();
        }

        // checks if there is no enemies on payload and if the payload has health
        if (isMoving && !enemyOnPayload && health > 50)
            transform.position = Vector3.MoveTowards(transform.position, path[currentWayPoint].position, Time.deltaTime * speed);

        // checks if the payload has reached the next checkpoint
        if (dist <= reachDist)
        {
            currentWayPoint++; // tells the payload to head to the next waypoint
            StartRotating(); // gives the payload a smooth turning
        }

        // ends the objective
        if (currentWayPoint >= path.Length)
        {
            if (!objectiveCompleted)
                GameManager.completedObjectives++; // adds to the amount of completed objectives
            
            objectiveCompleted = true;
            gameObject.SetActive(false);
        }

        // stops the healing when there are enemies on the payload
        else
            CancelInvoke();

        // if the payload goes above max health, gets set back to max health
        if (health > maxHealth)
            health = maxHealth;

        if (health <= 0)
            Die();

        // lets the payload move if no guns are being shot
        if (!gun.isShooting && !gun.isShootingBlaster)
            isMoving = true;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
                enemies.Remove(enemy);

            return;
        }

    }

    void MissionFail()
    {
        player.Die(); // sets the player back to spawn
        gameObject.SetActive(false); // sets the mission as in active
    }

    // allows the waypoints to be seen for ease of testing
    private void OnDrawGizmos()
    {
        if(path.Length >0 )
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] != null)
                    Gizmos.DrawSphere(path[i].position, reachDist);
            }
    }

    // sets the payload to start looking at the next waypoint
    public void StartRotating()
    {
        // checks if the payload has already started rotating and stops if it has
        if(LookCoroutine != null)
            StopCoroutine(LookCoroutine);

        LookCoroutine = StartCoroutine(LookAt());
    }

    IEnumerator LookAt()
    {
        // slows the rotating to make it less of a harsh turn
        Quaternion lookRotation = Quaternion.LookRotation(path[currentWayPoint].position - transform.position);
        float time = 0;

        // only turns if the time is above 1
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            enemies.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
            enemies.Remove(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        // checks if the player is on the payload and only moves if the player is shooting the blaster
        if (other.tag == "Player")
        {
            if (gun.isShooting && gun.isShootingBlaster)
                isMoving = true; 

           if (gun.isShooting && !gun.isShootingBlaster)
                isMoving = false;
        }
    }

    // removes 1 from the enemy coutner
    void MinusEnemy()
    {
        enemiesOnPayload--;
    }

    // lets the payload slowly heal until it reaches max health
    void StartHealing()
    {
        print("Healing");

        time += Time.deltaTime;

        if (time >= regenTime)
        {
            time = time - regenTime;
            if (health < maxHealth)
                health += healthRegen;
        }
    }

    // the payload takes damage
    public void TakeDamage(float amount)
    {
        beingAttacked = true;
        health -= amount;
        if (health <= 0f)
            Die();
    }

    // the objective ends
    void Die()
    {
        MissionFail();
    }

}
