using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    private List<Joycon> joycons;
    private int jc_ind = 0;

    public int selectedWeapon = 0;
    public string equippedWeapon;

    public Animator blasterAnimator; // pistol
    public Animator zingerAnimator; // semi-auto
    public Animator vaporizerAnimator; // sniper
    public Animator disruptorAnimator; // shotgun

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        // sets the starting variables
        joycons = JoyconManager.Instance.j;
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        Joycon j = joycons[jc_ind];

        if (j.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.A) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.D) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.X) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }

        if (previousSelectedWeapon != selectedWeapon) 
            SelectWeapon(); 

        // UI animations 
        if(selectedWeapon == 0)
        {
            equippedWeapon = "Blaster";
            blasterAnimator.SetTrigger("Selected");
            zingerAnimator.SetTrigger("Deselected");
            vaporizerAnimator.SetTrigger("Deselected");
            disruptorAnimator.SetTrigger("Deselected");
        }

        if (selectedWeapon == 1)
        {
            equippedWeapon = "Zinger";
            blasterAnimator.SetTrigger("Deselected");
            zingerAnimator.SetTrigger("Selected");
            vaporizerAnimator.SetTrigger("Deselected");
            disruptorAnimator.SetTrigger("Deselected");
        }

        if (selectedWeapon == 2)
        {
            equippedWeapon = "Vaporizer";
            blasterAnimator.SetTrigger("Deselected");
            zingerAnimator.SetTrigger("Deselected");
            vaporizerAnimator.SetTrigger("Selected");
            disruptorAnimator.SetTrigger("Deselected");
        }

        if (selectedWeapon == 3)
        {
            equippedWeapon = "Disruptor";
            blasterAnimator.SetTrigger("Deselected");
            zingerAnimator.SetTrigger("Deselected");
            vaporizerAnimator.SetTrigger("Deselected");
            disruptorAnimator.SetTrigger("Selected");
        }

    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if(i == selectedWeapon)
                weapon.gameObject.SetActive(true);  
            else
                weapon.gameObject.SetActive(false); 
            i++;
        }
    }
}
