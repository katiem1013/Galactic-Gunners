using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceDoor : MonoBehaviour
{
    public bool leaveDoor;

    private void OnEnable()
    {
        GameManager.escortDeaths = 0;
        GameManager.escortKills = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (leaveDoor)
            SceneManager.LoadScene("Collect Objective");
    }

    private void OnTriggerEnter(Collider other)
    {
        leaveDoor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        leaveDoor = false;
    }
}
