using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Objectives")]
    static public float completedObjectives = 0;
    public string currentObjective;
    static public string savedCurrentObjective;
    public bool levelIsLeaveable;

    [Header("Objective Stats")]
    public bool canAdd;
    public TextMesh escortDeathText, escortKillText;
    public TextMesh collectDeathText, collectKillText;
    public TextMesh destroyDeathText, destroyKillText;
    public TextMesh protectDeathText, protectKillText;
    static public int escortKills, escortDeaths;
    static public int collectKills, collectDeaths;
    static public int destroyKills, destroyDeaths;
    static public int protectKills, protectDeaths;

    [Header("Boss")]
    public GameObject boss;

    [Header("Health Packs")]
    public GameObject healthCollidedObject;
    public List<GameObject> healthCollidedList;

    [Header("Ammo Packs")]
    public GameObject ammoCollidedObject;
    public List<GameObject> ammoCollidedList;

    [Header("Explosive Barrel")]
    public GameObject barrelCollidedObject;
    public List<GameObject> barrelCollidedList;

    public static GameManager instance;

    // makes sure the script
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentObjective = savedCurrentObjective;
    }

    private void Update()
    {
        savedCurrentObjective = currentObjective;

        if (completedObjectives == 4 || (Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.I)))
        {
            currentObjective = "Boss";
            boss.SetActive(true);
        }

        // sets the text for all the objectives
        escortDeathText.text = "Deaths: " + escortDeaths.ToString();
        escortKillText.text = "Kills: " + escortKills.ToString();
        collectDeathText.text = "Deaths: " + collectDeaths.ToString();
        collectKillText.text = "Kills: " + collectKills.ToString();
        destroyDeathText.text = "Deaths: " + destroyDeaths.ToString();
        destroyKillText.text = "Kills: " + destroyKills.ToString();
        protectDeathText.text = "Deaths: " + protectDeaths.ToString();
        protectKillText.text = "Kills: " + protectKills.ToString();
    }

    public void HideHealthPack()
    {
        if (healthCollidedObject != null)
        {
            healthCollidedObject.SetActive(false);
            healthCollidedList.Add(healthCollidedObject);
            Invoke("HealthRespawn", 15);
        }
    }
    public void HealthRespawn()
    {
        for(int i = 0; i < healthCollidedList.Count; i++)
        {
            healthCollidedList[i].SetActive(true);
            healthCollidedList.Remove(healthCollidedList[i]);
        }
    }

    public void HideAmmoPack()
    {
        if (ammoCollidedObject != null)
        {
            ammoCollidedObject.SetActive(false);
            ammoCollidedList.Add(ammoCollidedObject);
            Invoke("AmmoRespawn", 15);
        }
    }
    public void AmmoRespawn()
    {
        for (int i = 0; i < ammoCollidedList.Count; i++)
        {
            ammoCollidedList[i].SetActive(true);
            ammoCollidedList.Remove(ammoCollidedList[i]);
        }
    }

    public void HideBarrel()
    {
        if (barrelCollidedObject != null)
        {
            barrelCollidedObject.SetActive(false);
            barrelCollidedList.Add(barrelCollidedObject);
            Invoke("BarrlRespawn", 15);
        }
    }
    public void BarrelRespawn()
    {
        for (int i = 0; i < barrelCollidedList.Count; i++)
        {
            barrelCollidedList[i].SetActive(true);
            barrelCollidedList.Remove(barrelCollidedList[i]);
        }
    }

    public void StartAdding()
    {
        if (canAdd)
        {
            canAdd = false;
            Invoke("AddKills", 0.1f);
        }
    }

    public void AddKills()
    {
        if (currentObjective == "Payload")
            escortKills++;

        if (currentObjective == "Collect")
            collectKills++;

        if (currentObjective == "Destroy")
            destroyKills++;

        if (currentObjective == "Protect")
            protectKills++;
    }
}
