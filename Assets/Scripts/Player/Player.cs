using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameManager gameManager;
    public MissionSelect missionSelect;

    [Header("Joycons")]
    private List<Joycon> joycons;
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public int jc_ind = 0;
    public Quaternion orientation;

    [Header("Movement")]
    public float speed = 10f;
    public Rigidbody rb;
    public Vector3 movement;
    public Vector3 startingPos;

    [Header("Health")]
    public float playerHealth = 100f;
    public float playerMaxHealth;
    public Text health;
    

    void Start()
    {
        // sets starting variables
        rb = this.GetComponent<Rigidbody>();
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);

        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
            Destroy(gameObject); // destroys if theres more than the connect controllers

        startingPos = gameObject.transform.position; // sets the respawn position
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // adds the players health to the UI
        health.text = playerHealth.ToString();

        // if the players health goes over max health it gets set to max health
        if (playerHealth > playerMaxHealth)
            playerHealth = playerMaxHealth;

        if (playerHealth <= 0)
            Die();

        Joycon j = joycons[jc_ind];
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > 0)
        {
            stick = j.GetStick();

            // finds the rotation of the joycons
            orientation = j.GetVector();
            orientation = new Quaternion((orientation.x * -1), (orientation.z), orientation.y * -1, (orientation.w * -1));

            gameObject.transform.rotation = orientation;

            // sends a raycast out from the screen to where the joycons are facing
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(orientation.x, orientation.y, orientation.z)); 
            RaycastHit hit; // variable for the object that has been hit so it can be referenced later
            if (Physics.Raycast(ray, out hit, 0))
                this.transform.LookAt(hit.point); // sets the camera to look at the point the raycast hits

            // checking if the player is moving the joystick and moves the character in that direction
            movement = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(j.GetStick()[0], 0, j.GetStick()[1]);
            moveCharacter(movement);
        }
    }

    // moves the character in the speed and direction 
    void moveCharacter(Vector3 direction)
    {
        rb.velocity = direction * speed;
        rb.AddForce(new Vector3(0,-100,0));
    }

    // the player takes damage
    public void TakeDamage(float amount)
    {
        playerHealth -= amount;
        
        // kills the player if their health hits zero
        if (playerHealth <= 0f)
            Die();
    }

    // the player dies and health gets set back
    public void Die()
    {
        transform.position = startingPos;
        playerHealth = playerMaxHealth;

        if (gameManager.currentObjective == "Payload")
            GameManager.escortDeaths++;

        if (gameManager.currentObjective == "Collect")
            GameManager.collectDeaths++;

        if (gameManager.currentObjective == "Destroy")
            GameManager.destroyDeaths++;

        if (gameManager.currentObjective == "Protect")
            GameManager.protectDeaths++;
    }
}