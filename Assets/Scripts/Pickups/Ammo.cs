using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Weapons")]
    public WeaponSwitching weaponSwitching;
    public string weaponAmmo;
    public Gun blaster;
    public Gun zinger;
    public Gun vaporizer;
    public Gun disruptor;
   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(weaponSwitching.equippedWeapon == "Blaster" && weaponAmmo == "Blaster")
            {
                blaster.currentAmmo = blaster.maxAmmo;
                blaster.currentMaxAmmo = 120;
            }
                

            if (weaponSwitching.equippedWeapon == "Zinger" && weaponAmmo == "Zinger")
            { 
                zinger.currentAmmo = zinger.maxAmmo;
                zinger.currentMaxAmmo = 300;
            }

            if (weaponSwitching.equippedWeapon == "Vaporizer" && weaponAmmo == "Vaporizer")
            { 
                vaporizer.currentAmmo = vaporizer.maxAmmo;
                vaporizer.currentMaxAmmo = 10;
            }
            if (weaponSwitching.equippedWeapon == "Disruptor" && weaponAmmo == "Disruptor")
            { 
                disruptor.currentAmmo = disruptor.maxAmmo;
                disruptor.currentMaxAmmo = 40;
            }

            if(weaponAmmo == "All")
            {
                blaster.currentAmmo = blaster.maxAmmo;
                blaster.currentMaxAmmo = 120;

                zinger.currentAmmo = zinger.maxAmmo;
                zinger.currentMaxAmmo = 300;

                vaporizer.currentAmmo = vaporizer.maxAmmo;
                vaporizer.currentMaxAmmo = 10;

                disruptor.currentAmmo = disruptor.maxAmmo;
                disruptor.currentMaxAmmo = 40;
            }

            gameManager.ammoCollidedObject = this.gameObject;
            gameManager.HideAmmoPack();
        }
    }
}
