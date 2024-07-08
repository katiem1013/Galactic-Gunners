using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public string enemyType;

    [Header("Attackable Objects")]
    public Player player;
    public Payload payload;
    public Protect attackWall;

    [Header("Splat Attack")]
    public GameObject splat;
    public GameManager gameManager;

    [Header("Random Attack")]
    public float randNum;
    public float badAim;

    private void Start()
    {
    }

    private void Update()
    {
        // will randomly make the Vylak miss shots
        if (enemyType == "Vylak")
        {
            randNum = Random.Range(0, 100); // gets a random number
            if (randNum > 85) // checks if the random number is above 85% 
            {
                badAim = Random.Range(5, 10); // decides where the enemy is shooting
                transform.Translate((Vector3.forward + (new Vector3(badAim, badAim, badAim))) * Time.deltaTime * 10); // makes the enemy more like to miss 
                Destroy(gameObject, 1); // destroys the bullet after 1 second
            }

            else // if the random number is within 85% the enemy hits the player 
            {
                transform.Translate(Vector3.forward * Time.deltaTime * 10); // shoots at the player
                Destroy(gameObject, 1); // destroys the bullet after 1 second
            }
        }
    }

    private void OnCollisionEnter(Collision collide)
    {
        player = GameObject.FindObjectOfType(typeof(Player)) as Player; // gets the player object
        payload = GameObject.FindObjectOfType(typeof(Payload)) as Payload; // gets the payload object
        attackWall = GameObject.FindObjectOfType(typeof(Protect)) as Protect;


        // deals damage to the player if hit, and destroys the bullet
        if (collide.gameObject.CompareTag("Player"))
        {
            // multiplys the enemy damage by the amount of objectives completed
            if (GameManager.completedObjectives != 0)
                player.TakeDamage(damage * (1.1f * (GameManager.completedObjectives + 1))); // adds 1 so that it still does extra damage when only one objective is completed

            else
                player.TakeDamage(damage); // does base damage when 0 objectives have been done

            Destroy(gameObject); // destroys the bullet
        }

        // deals damage to the payload
        if (collide.gameObject.CompareTag("Payload"))
        {
            payload.TakeDamage(damage / 2);
            Destroy(gameObject); // destroys the bullet
        }

        // deals damage to the payload
        if (collide.gameObject.CompareTag("Protect"))
        {
            attackWall.TakeDamage(damage);
            Destroy(gameObject);
        }

        // if the bullet hits the ground it will spawn a splatter that deals damage to the player.
        if (collide.gameObject.CompareTag("Ground"))
        {
            // checks if the nemi is shooting
            if (enemyType == "Nemi")
            {
                // gets the contact point of the bullet
                ContactPoint contact = collide.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                Instantiate(splat, pos, rot);// spawns the splatter
                Destroy(gameObject); // destroys the bullet
            }
        }
    }
}
