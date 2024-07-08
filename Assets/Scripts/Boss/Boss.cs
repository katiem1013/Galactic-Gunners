using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Boss Phase")]
    public float phase = 1;
    public bool shielded, beingDamaged;

    [Header("Health")]
    public float health;
    public float maxHealth;
    public float healthRegen;
    private float time = 0.0f;
    public float regenTime = 1f;

    [Header("Attacks")]
    public float attackThreeDamage;
    public float attackPattern, bulletTime, timeBetweenAttacks;
    private bool alreadyAttacked, reversing;
    public GameObject bossBulletOne, bossBulletTwo, beamAttack;
    public Transform spawnPoint;
    public Transform[] attackPoints;
    public float attackSpeed, bulletAmount;

    [Header("Attackable Objects")]
    public Player player;

    [Header("Movement")]
    public Transform[] attackOnePath;
    public Transform[] attackTwoPath;
    public Transform[] attackThreePath; 
    public float attackOneSpeed, attackTwoSpeed, attackThreeSpeed;
    public float reachDist = 1;
    public int currentWayPoint = 0;

    [Header("Disable Objects")]
    public GameObject spawners;
    public GameObject enviroment;

    // Start is called before the first frame update
    void Start()
    {
        health = 750;
        spawners.SetActive(false);
        enviroment.SetActive(false);
        StartCoroutine(NumberGen());
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == 1)
        {
            maxHealth = 750;

            if (health <= 0f)
            {
                phase = 2;
                maxHealth = 250;
                health = 250;
            }

            // checks which attack pattern the boss should currently be in 
            if (attackPattern == 1)
            {
                AttackPatternOne();
                beamAttack.SetActive(false);
            }

            if (attackPattern == 2 && !reversing)
            {
                AttackPatternTwo();
                beamAttack.SetActive(true);
            }

            if (attackPattern == 2 && reversing)
            {
                Reverse();
                beamAttack.SetActive(true);
            }

            if (attackPattern == 3)
            {
                AttackPatternThree();
                beamAttack.SetActive(false);
            }
        }

        if (phase == 2)
        {

            Invoke("Phase", 0);
            AttackPatternFour();
            beamAttack.SetActive(false);

            if (!beingDamaged)
                Invoke("StartHealing", 8f);

            if (health <= 0f)
                Invoke("Die", 0.1f);
        }
    }
    
    void Phase2()
    {
        health = 250;
        attackPattern = 4;
    }

    void AttackPatternOne() 
    {
        float dist = Vector3.Distance(attackOnePath[currentWayPoint].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, attackOnePath[currentWayPoint].position, Time.deltaTime * attackOneSpeed);

        if (dist <= reachDist)
            currentWayPoint++;

        if (currentWayPoint > attackOnePath.Length)
            currentWayPoint = 0;

        if (!alreadyAttacked)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime > 0)
                return;

            bulletTime = timeBetweenAttacks;

            GameObject bulletObject = Instantiate(bossBulletOne, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            Rigidbody bulletRB = bulletObject.GetComponent<Rigidbody>();
            bulletRB.AddForce(bulletRB.transform.forward * 1000);
            bulletObject.SetActive(true);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // resets the attack
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        bulletAmount = 0;
    }

    void AttackPatternTwo() 
    {
        float dist = Vector3.Distance(attackTwoPath[currentWayPoint].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, attackTwoPath[currentWayPoint].position, Time.deltaTime * attackTwoSpeed);

        if (dist <= reachDist)
            currentWayPoint++;

        if (currentWayPoint == attackTwoPath.Length)
        {
            reversing = true;
            currentWayPoint--;
        }
    }

    void Reverse()
    {
        float dist = Vector3.Distance(attackTwoPath[currentWayPoint].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, attackTwoPath[currentWayPoint].position, Time.deltaTime * attackTwoSpeed);

        if (dist <= reachDist)
            currentWayPoint--;

        if (currentWayPoint <= 0)
            reversing = false;
    }

    // four direction attack
    void AttackPatternThree() 
    {
        float dist = Vector3.Distance(attackThreePath[currentWayPoint].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, attackThreePath[currentWayPoint].position, Time.deltaTime * attackThreeSpeed);

        if (dist <= reachDist)
            Invoke("NextWayPoint", 0.5f);

        if (currentWayPoint >= attackThreePath.Length)
            currentWayPoint = 0;

        if (!alreadyAttacked)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime > 0)
                return;

            bulletTime = timeBetweenAttacks;

            if (bulletAmount == 0)
            {
                GameObject bulletObject = Instantiate(bossBulletTwo, spawnPoint.transform.position, spawnPoint.transform.rotation);
                bulletObject.SetActive(true);
                bulletObject.transform.eulerAngles = new Vector3(0, 0, 0);
                bulletAmount = 1;
            }

            if (bulletAmount == 1)
            {
                GameObject bulletObject = Instantiate(bossBulletTwo, spawnPoint.transform.position, spawnPoint.transform.rotation);
                bulletObject.SetActive(true);
                bulletObject.transform.eulerAngles = new Vector3(0, 90, 0);
                bulletAmount = 2;
            }

            if (bulletAmount == 2)
            {
                GameObject bulletObject = Instantiate(bossBulletTwo, spawnPoint.transform.position, spawnPoint.transform.rotation);
                bulletObject.SetActive(true);
                bulletObject.transform.eulerAngles = new Vector3(0, 180, 0);
                bulletAmount = 3;
            }

            if (bulletAmount == 3)
            {
                GameObject bulletObject = Instantiate(bossBulletTwo, spawnPoint.transform.position, spawnPoint.transform.rotation);
                bulletObject.SetActive(true);
                bulletObject.transform.eulerAngles = new Vector3(0, 270, 0);
                bulletAmount = 4;
            }

            if (bulletAmount == 4)
            {
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks); // resets the attack
            }

        }
    }

    void AttackPatternFour()
    {
        float dist = Vector3.Distance(attackThreePath[currentWayPoint].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, attackThreePath[currentWayPoint].position, Time.deltaTime * attackThreeSpeed);

        if (dist <= reachDist)
        {
            shielded = true; 
            Invoke("NextWayPoint", 0.5f);
        }

        if (currentWayPoint >= attackThreePath.Length)
            currentWayPoint = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        player = GameObject.FindObjectOfType(typeof(Player)) as Player; // gets the player object

        // deals damage to the player if hit
        if (other.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(attackThreeDamage);
        }
    }

    void NextWayPoint()
    {
        shielded = false;

        if (attackPattern != 3 && attackPattern == 4)
            currentWayPoint++;

        else if (attackPattern == 3 || attackPattern == 4)
            currentWayPoint = Random.Range(0, attackThreePath.Length);
    }

    public void TakeDamage(float amount)
    {
        if (phase == 1)
            health -= amount;

        if (phase == 2 && shielded)
            health -= amount / 2;

        if (phase == 2 && !shielded)
            health -= amount;

        beingDamaged = true;
        Invoke("StopDamage", 8);
    }

    void StopDamage()
    {
        beingDamaged = false;
    }

    IEnumerator Die()
    {
        Destroy(gameObject);
        return null;
    }

    // lets the boss slowly heal until it reaches max health
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
    IEnumerator NumberGen()
    {
        while (true)
        {
            attackPattern = Random.Range(1, 3);
            yield return new WaitForSeconds(10);
        }
    }

}
