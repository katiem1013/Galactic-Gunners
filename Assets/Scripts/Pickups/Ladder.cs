using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Joycons")]
    private List<Joycon> joycons;
    public float[] stick;
    public int jc_ind = 0;

    [Header("Ladders")]
    public Transform playerTransform;
    public bool onLadder;
    public float ladderSpeed;
    public Player player;

    void Start()
    {
        // sets the starting variables
        player = GetComponent<Player>();
        onLadder = false;

        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
            Destroy(gameObject); // destroys if theres more than the connect controllers

    }

    void OnTriggerEnter(Collider other)
    {
        // disables the player controls if the player gets on the ladder
        if (other.gameObject.CompareTag("Ladder"))
        {
            player.enabled = false;
            onLadder = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // enables the player controls when they get off the ladder
        if (other.gameObject.CompareTag("Ladder"))
        {
            player.enabled = true;
            onLadder = false;
        }
    }

    private void Update()
    {
        Joycon j = joycons[jc_ind];
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > 0)
        {
            stick = j.GetStick();

            if (onLadder == true && j.GetStick()[1] > 0)
                playerTransform.transform.position += Vector3.up / (ladderSpeed/2);

            if (onLadder == true && j.GetStick()[1] <= 0)
                playerTransform.transform.position += Vector3.down / ladderSpeed;
        }
    }
}
