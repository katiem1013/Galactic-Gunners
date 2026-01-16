using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public Player player;

    [Header("Damage")]
    public float attackDamage;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.GetComponent<Player>().TakeDamage(attackDamage);

    }
}
