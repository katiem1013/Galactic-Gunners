using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameManager gameManager;
    [Header("Checking Layers")]
    public LayerMask whatIsPlayer;
    public LayerMask whatIsNemi, whatIsOrzeth, whatIsVylak, whatIsZaryx, whatIsRaxor;

    [Header("Get Player and Enemy Health")]
    public Player player;
    public Target nemi, orzeth, zaryx, vylak, raxor;
    private bool playerInAttackRange, nemiInAttackRange, orzethInAttackRange, vylakInAttackRange, zaryxInAttackRange, raxorInAttackRange;

    [Header("Explosion")]
    public float damageRange;
    public float explosionDamge;
    public bool exploded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the player and enemies are within the barrel explosion range
        playerInAttackRange = Physics.CheckSphere(transform.position, damageRange, whatIsPlayer);
        nemiInAttackRange = Physics.CheckSphere(transform.position, damageRange, whatIsNemi);
        orzethInAttackRange = Physics.CheckSphere(transform.position, damageRange, whatIsOrzeth);
        zaryxInAttackRange = Physics.CheckSphere(transform.position, damageRange, whatIsZaryx);
        vylakInAttackRange = Physics.CheckSphere(transform.position, damageRange, whatIsVylak);
        raxorInAttackRange = Physics.CheckSphere(transform.position, damageRange, whatIsRaxor);

        if (playerInAttackRange && exploded)
        {
            player.playerHealth -= (explosionDamge / 2);
            Destroy(gameObject);
            gameManager.barrelCollidedObject = this.gameObject;
            gameManager.HideBarrel();
        }

        if (nemiInAttackRange && exploded)
        { 
            nemi.health -= explosionDamge;
            Destroy(gameObject);
            gameManager.barrelCollidedObject = this.gameObject;
            gameManager.HideBarrel();
        }

        if (orzethInAttackRange && exploded)
        { 
            orzeth.health -= explosionDamge;
            Destroy(gameObject);
            gameManager.barrelCollidedObject = this.gameObject;
            gameManager.HideBarrel();
        }

        if (zaryxInAttackRange && exploded)
        { 
            zaryx.health -= explosionDamge; 
            Destroy(gameObject);
            gameManager.barrelCollidedObject = this.gameObject;
            gameManager.HideBarrel();
        }

        if (vylakInAttackRange && exploded)
        { 
            vylak.health -= explosionDamge;
            Destroy(gameObject);
            gameManager.barrelCollidedObject = this.gameObject;
            gameManager.HideBarrel();
        }

        if (raxorInAttackRange && exploded)
        { 
            raxor.health -= explosionDamge;
            Destroy(gameObject);
            gameManager.barrelCollidedObject = this.gameObject;
            gameManager.HideBarrel();
        }
    }
}
