using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Target Range")]
    public Transform target;
    public float range = 30f;

    [Header("Shooting")]
    public float damage;
    public float fireRate, timeToNextFire;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f); // slows down the turret finding the enemies so that it doesn't cause lag
    }

    // Update is called once per frame
    void Update()
    {
        // does nothing if there is no target
        if (target == null)
            return;

        // checks if its time to shoot
        if (timeToNextFire <= 0)
        {
            Shoot(); // shoots the turret
            timeToNextFire = 1 / fireRate; // sets the timeToNextFire based on the fire rate
        }

        timeToNextFire -= Time.deltaTime; // reduces timeToNextFire every second
    }

    void Shoot()
    {
        RaycastHit gunHit;
        if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.forward, out gunHit, 15))
        {
            Target shootTarget = gunHit.transform.GetComponent<Target>();

            if (shootTarget != null)
                shootTarget.TakeDamage(damage);

            print("Attacked");
        }
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // finds all the enemies in the scene and adds them to a list
        float shortestDist = Mathf.Infinity; // figures out the shortest of an enemy, if there is no enemy it's an infinte distance away
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies) 
        {
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position); // checks the position of every enemy in the scene in comparison to the turret
            if (distToEnemy < shortestDist) // finds the closest enemy
            {
                // sets this as the closet enemy so far
                shortestDist = distToEnemy;   
                nearestEnemy = enemy; 
            }
        }

        // checks if an enemy is found and with the turret range
        if (nearestEnemy != null && shortestDist <= range)
            target = nearestEnemy.transform; // sets this as the target

        else
            target = null; // gets rid of the target if it goes out of range
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
