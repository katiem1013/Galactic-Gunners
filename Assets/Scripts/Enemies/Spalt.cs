using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spalt : MonoBehaviour
{
    public float damage;

    [Header("Attackable Objects")]
    public Player player;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die", 6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        player = GameObject.FindObjectOfType(typeof(Player)) as Player;

        if (other.gameObject.CompareTag("Player"))
            player.TakeDamage(damage);

    }

    public void Die()
    { 
        Destroy(gameObject);
    }
}
