using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damage;

    [Header("Attackable Objects")]
    public Player player;

    [Header("Splat Attack")]
    public GameObject splat;

    private void OnCollisionEnter(Collision collide)
    {
        player = GameObject.FindObjectOfType(typeof(Player)) as Player; // gets the player object

        // deals damage to the player if hit, and destroys the bullet
        if (collide.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }

        // if the bullet hits the ground it will spawn a splatter that deals damage to the player.
        if (collide.gameObject.CompareTag("Ground"))
        {
            // gets the contact point of the bullet
            ContactPoint contact = collide.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            pos += new Vector3(0, 0.5f, 0); // the splatter goes under the ground, this corrects that

            Instantiate(splat, pos, rot).SetActive(true); // spawns the splatter
            Destroy(gameObject); // destroys the bullet
        }
    }
}
