using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelect : MonoBehaviour
{

    public string selectableMission;
    public bool buttonPressed;

    public GameManager gameManager;

    [Header("Mission Objects")]
    public GameObject escort;
    public GameObject destroy;
    public GameObject collect;
    public GameObject protect;
    public Protect protectData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selectableMission == "Escort" && buttonPressed)
        {
            escort.SetActive(true);
            destroy.SetActive(false);
            collect.SetActive(false);
            protect.SetActive(false);
            buttonPressed = false;
        }

        if (selectableMission == "Destroy" && buttonPressed)
        {
            escort.SetActive(false);
            destroy.SetActive(true);
            collect.SetActive(false);
            protect.SetActive(false);
            buttonPressed = false;
        }


        if (selectableMission == "Collect" && buttonPressed)
        {
            escort.SetActive(false);
            destroy.SetActive(false);
            collect.SetActive(true);
            protect.SetActive(false);
            buttonPressed = false;
            protectData.protectActive = true;
        }

        if (selectableMission == "Protect" && buttonPressed)
        {
            escort.SetActive(false);
            destroy.SetActive(false);
            collect.SetActive(false);
            protect.SetActive(true);
            buttonPressed = false;
        }

        else
            protectData.protectActive = false;

    }
}
