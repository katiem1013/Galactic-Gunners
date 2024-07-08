using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zaryx : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform playerTransform;
    public LayerMask whatIsGround, whatIsPlayer, whatIsPayload;

    [Header("Reducing Sight")]
    public float lastKnownHealth;
    public float currentHealth;
    public Target target;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    [Header("Attacks")]
    private float bulletTime;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public float attackAcuracy;

    [Header("Ranges and Spawners")]
    public Transform spawnPoint;
    public enemySpawner enemy;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, payloadInAttackRange;

    [Header("Objectives")]
    public Payload payload;
    public Transform payloadTransform;
    public Transform protectTransform;
    public GameManager gameManager;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        payloadTransform = payload.transform;
    }

    private void Start()
    {
        lastKnownHealth = target.health;
    }

    private void Update()
    {
        currentHealth = target.health;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        payloadInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPayload);

        if (!playerInSightRange && !playerInAttackRange)
            Patrol();

        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        if (playerInSightRange && playerInAttackRange)
            AttackPlayer();

        if (payload.payloadActive == true && payloadInAttackRange)
            AttackPayload();

        if (currentHealth == (lastKnownHealth - 10))
        {
            sightRange -= (sightRange * 0.04f);
            attackRange -= (sightRange * 0.04f);
            lastKnownHealth = currentHealth;
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

            if (Random.value <= attackAcuracy)
            {
                Quaternion rotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(-20, 20), 0);
                var modifiedForwardDirection = rotation * Camera.main.transform.forward;

                RaycastHit gunHit;
                if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.forward + modifiedForwardDirection, out gunHit, 15))
                {
                    Player player = gunHit.transform.GetComponent<Player>();

                    if (player != null)
                        player.TakeDamage(5);

                    print("Not Attacked");
                }
            }

            else
            {
                RaycastHit gunHit;
                if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.forward, out gunHit, 15))
                {
                    Player player = gunHit.transform.GetComponent<Player>();

                    if (player != null)
                        player.TakeDamage(5);

                    print("Attacked");
                }
            }

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

            RaycastHit gunHit;
            if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.forward, out gunHit, 15))
            {
                Payload payload= gunHit.transform.GetComponent<Payload>();
                if (payload != null)
                    payload.health -= 50;
            }

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

            RaycastHit gunHit;
            if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.forward, out gunHit, 15))
            {
                Protect attackWall = gunHit.transform.GetComponent<Protect>();
                if (attackWall != null)
                    attackWall.health -= 50;
            }


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // resets the attack
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
