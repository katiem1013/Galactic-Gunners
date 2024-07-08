using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Player player;
    private void OnTriggerEnter(Collider other)
    {
        // kills the player if they fall off of the map
        if (other.gameObject.CompareTag("Player"))
            player.Die();
    }
}
