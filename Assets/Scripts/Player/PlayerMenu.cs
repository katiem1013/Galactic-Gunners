using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    private List<Joycon> joycons;
    public Quaternion orientation;
    private int jc_ind = 0;
    public Camera cam;
    public GameObject cursor;

    public TextMeshPro startText, exitText;
    public bool startPressed, exitPressed;
    public GameObject controlScreen;
    public GameObject mainMenu;

    void Start()
    {
        // sets the starting variables
        joycons = JoyconManager.Instance.j;

    }

    private void Update()
    {
        Joycon j = joycons[jc_ind];
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > 0)
        {
            if (j.GetButton(Joycon.Button.SHOULDER_2)) // checks if the player presses the trigger
            {
                TriggerPressed();
            }
        }

        Vector2 stickDirection = new Vector2(j.GetStick()[0], j.GetStick()[1]);

        if (stickDirection.x < 0)
        {
            startPressed = true;
            exitPressed = false;
            startText.color = new Color32(20, 70, 20, 255);
            exitText.color = new Color32(255,255,255,255);
        }

        if (stickDirection.x > 0)
        {
            exitPressed = true;
            startPressed = false;
            startText.color = new Color32(255, 255, 255, 255);
            exitText.color = new Color32(20, 70, 20, 255);
        }

    }

    void TriggerPressed()
    {
        if (exitPressed)
            Application.Quit();

        if (startPressed)
        {
            controlScreen.SetActive(true);
            mainMenu.SetActive(false);
        }

        if (controlScreen != null)
            SceneManager.LoadScene("Scene1");

    }

}
