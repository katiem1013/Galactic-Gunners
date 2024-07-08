using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orzeth : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform playerTransform;

    [Header("Checking Layers")]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer, whatIsNemi, whatIsVylak, whatIsZaryx, whatIsRaxor, whatIsPayload;

    [Header("Get Player and Enemy Health")]
    public Player player;
    public Target nemi, zaryx, vylak, raxor;

    [Header("Health")]
    public Target target;
    public bool canHeal;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    [Header("Attacks")]
    private float bulletTime;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject enemyBullet;

    [Header("Ranges and Spawners")]
    public Transform spawnPoint;
    public enemySpawner enemy;
    public float sightRange, attackRange, healRange;
    public bool playerInSightRange, playerInAttackRange, playerInHealRange, payloadInAttackRange;

    [Header("Objectives")]
    public Payload payload;
    public Transform payloadTransform;
    public Transform protectTransform;
    public GameManager gameManager;

    // enemy ranges
    private bool nemiInHealRange, vylakInHealRange, zaryxInHealRange, raxorInHealRange; 

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        payloadTransform = payload.transform;
    }

    private void Update()
    {
        // checks if the player (and payload) is within the ranges
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInHealRange = Physics.CheckSphere(transform.position, healRange, whatIsPlayer);
        payloadInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPayload);

        if (!playerInSightRange && !playerInAttackRange)
            Patrol();

        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        if (playerInSightRange && playerInAttackRange)
            AttackPlayer();

        if (payload.payloadActive == true && payloadInAttackRange)
            AttackPayload();

        // checks if the enemies are within the heal range
        nemiInHealRange = Physics.CheckSphere(transform.position, healRange, whatIsNemi);
        zaryxInHealRange = Physics.CheckSphere(transform.position, healRange, whatIsZaryx);
        vylakInHealRange = Physics.CheckSphere(transform.position, healRange, whatIsVylak);
        raxorInHealRange = Physics.CheckSphere(transform.position, healRange, whatIsRaxor);

        if (playerInHealRange && target.health <= 0 && canHeal == true)
        {
            player.playerHealth += 10;
            canHeal = false;
            if (player.playerHealth > 150)
                player.playerHealth = 150;
        }

        if (nemiInHealRange && target.health <= 0 && canHeal == true)
        {
            nemi.health += 15f;
            if (nemi.health > 50)
                nemi.health = 50;
        }

        if (zaryxInHealRange && target.health <= 0 && canHeal == true)
        {
            zaryx.health += 15f;
            if (zaryx.health > 50)
                zaryx.health = 50;
        }

        if (vylakInHealRange && target.health <= 0 && canHeal == true)
        {
            vylak.health += 15f;
            if (vylak.health > 50)
                vylak.health = 50;
        }

        if (raxorInHealRange && target.health <= 0 && canHeal == true)
        {
            raxor.health += 15f;
            if (raxor.health > 50)
                raxor.health = 50;
        }

        if (payloadInAttackRange && payload.health <= 0 && canHeal == true)
        {
            payload.health += 15f;
            if (payload.health > 200)
                payload.health = 200;
        }

    }

    private void Patrol()
    {
        // when the walk point is false searches for the next position to walk to
        if (!walkPointSet)
            SearchWalkPoint();

        // once a position has been found the enemy will move to it
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        // calculating distance
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // when walkPoint is reached sets walk point to false
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // finds random points in range
        float randomz = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);

        // sets the position for the enemy to move to 
        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z + randomz);

        // checks if the enemy is on the ground 
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        // enemy chases the player
        agent.SetDestination(playerTransform.position);
    }

    private void AttackPlayer()
    {
        // keeps the player chasing the player and has them face the player to attack
        agent.SetDestination(playerTransform.position);
        transform.LookAt(playerTransform);

        // if the enemy hasn't attacked, start attacking
        if (!alreadyAttacked)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime > 0)
                return;

            bulletTime = timeBetweenAttacks;

            GameObject bulletObject = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            Rigidbody bulletRB = bulletObject.GetComponent<Rigidbody>();
            bulletRB.AddForce(bulletRB.transform.forward * 1000);
            Destroy(bulletObject, 2f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // resets the attack
        }
    }


    private void AttackPayload()
    {
        agent.SetDestination(payloadTransform.position);
        transform.LookAt(payloadTransform);

        // if the enemy hasn't attacked, start attacking
        if (!alreadyAttacked)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime > 0)
                return;

            bulletTime = timeBetweenAttacks;

            GameObject bulletObject = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            Rigidbody bulletRB = bulletObject.GetComponent<Rigidbody>();
            bulletRB.AddForce(bulletRB.transform.forward * 1000);
            Destroy(bulletObject, 2f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // resets the attack
        }
    }

    private void AttackWall()
    {
        agent.SetDestination(protectTransform.position);
        transform.LookAt(protectTransform);

        // if the enemy hasn't attacked, start attacking
        if (!alreadyAttacked)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime > 0)
                return;

            bulletTime = timeBetweenAttacks;

            GameObject bulletObject = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            Rigidbody bulletRB = bulletObject.GetComponent<Rigidbody>();
            bulletRB.AddForce(bulletRB.transform.forward * 1000);
            Destroy(bulletObject, 2f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // resets the attack
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

}
